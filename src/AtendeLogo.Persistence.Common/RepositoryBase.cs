using System.Linq.Expressions;
using AtendeLogo.Domain.Helpers;
using AtendeLogo.Persistence.Common.Enums;
using AtendeLogo.Persistence.Common.Exceptions;
using AtendeLogo.Shared.Contracts;

namespace AtendeLogo.Persistence.Common;

public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : EntityBase
{
    private readonly DbContext _dbContext;
    private readonly IUserSessionAccessor _userSessionAccessor;
    private readonly TrackingOption _trackingOption;

    private readonly bool _isImplementDeletedInterface;
    private readonly bool _isImplementTenantOwnedInterface;

    private bool _isIncludeDeleted;

    protected virtual int DefaultMaxRecords { get; } = RepositoryConstants.DefaultMaxRecords;

    protected RepositoryBase(
        DbContext dbContext,
        IUserSessionAccessor userSessionAccessor,
        TrackingOption trackingOption)
    {
        Guard.NotNull(dbContext);

        _dbContext = dbContext;
        _trackingOption = trackingOption;
        _userSessionAccessor = userSessionAccessor;

        _isImplementDeletedInterface = EntityReflectionHelper.IsImplementDeletedInterface<TEntity>();
        _isImplementTenantOwnedInterface = EntityReflectionHelper.IsImplementTenantOwnedInterface<TEntity>();
    }

    #region Queries
    public async Task<TEntity?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object?>>[] includeExpressions)
    {
        return await CreateQuery(includeExpressions)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<TEntity?> FindAsync(
        Expression<Func<TEntity, bool>> filterExpression,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object?>>[] includeExpressions)
    {
        Guard.NotNull(filterExpression);

        return await CreateQuery(includeExpressions)
            .FirstOrDefaultAsync(filterExpression, cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> FindAllAsync(
        Expression<Func<TEntity, bool>> filterExpression,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object?>>[] includeExpressions)
    {
        Guard.NotNull(filterExpression);

        return await CreateQuery(includeExpressions)
            .Where(filterExpression)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object?>>[] includeExpressions)
    {
        return await CreateQuery(includeExpressions)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> filterExpression,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object?>>[] includeExpressions)
    {
        Guard.NotNull(filterExpression);

        return await CreateQuery(includeExpressions)
            .AnyAsync(filterExpression, cancellationToken);
    }

    #endregion

    public async Task<TEntity> RefreshAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        Guard.NotNull(entity);

        var refreshedEntity = await TryRefreshAsync(entity, cancellationToken);

        return refreshedEntity is not null
            ? refreshedEntity
            : throw new EntityNotFoundException(typeof(TEntity), entity!.Id);
    }

    public async Task<TEntity?> TryRefreshAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        Guard.NotNull(entity);

        if (entity.Id == Guid.Empty)
            throw new ArgumentException("Is not possible to refresh entity with new entity.");

        var entry = _dbContext.Entry(entity);

        if (entry.State == EntityState.Detached &&
            _trackingOption == TrackingOption.Tracking)
        {
            RemoveAnyEntityTracked(entity);
            return await GetByIdAsync(entity.Id, cancellationToken);
        }

        await entry.ReloadAsync(cancellationToken);
        return entity;

    }

    private void RemoveAnyEntityTracked(TEntity entity)
    {
        var entry = _dbContext.ChangeTracker.Entries<TEntity>()
            .FirstOrDefault(x => x.Entity.Id == entity.Id);

        if (entry != null)
        {
            entry.State = EntityState.Detached;
        }
    }

    public void IncludeDeleted()
    {
        _isIncludeDeleted = true;
    }

    #region Filter

    protected virtual IQueryable<TEntity> CreateQuery(
        Expression<Func<TEntity, object?>>[]? includeExpressions)
    {
        var query = _dbContext.Set<TEntity>()
              .ApplyTracking(_trackingOption)
              .Take(DefaultMaxRecords);

        if (includeExpressions is not null)
        {
            query = includeExpressions.Aggregate(query, (current, include) => current.Include(include));
        }

        if (_isImplementTenantOwnedInterface &&
            ShouldFilterTenantOwned())
        {
            var userSession = _userSessionAccessor.GetCurrentSession();

            query = query.Cast<ITenantOwned>()
               .Where(x => x.Tenant_Id == userSession.Tenant_Id)
               .Cast<TEntity>();
        }

        if (_isImplementDeletedInterface && !_isIncludeDeleted)
        {
            query = query.Cast<ISoftDeletableEntity>()
                .Where(x => !x.IsDeleted)
                .Cast<TEntity>();
        }
        return query;
    }

    protected virtual bool ShouldFilterTenantOwned()
    {
        var userSession = _userSessionAccessor.GetCurrentSession();
        if (!userSession.IsTenantUser() && !userSession.IsSystemAdminUser())
        {
            throw new UnauthorizedAccessException("User not have permission to access this resource.");
        }
        return true;
    }

    protected IUserSession GetCurrentSession()
    {
        return _userSessionAccessor.GetCurrentSession();
    }

    protected IEndpointService? GetCurrentEndpointInstance()
    {
        return _userSessionAccessor.GetCurrentEndpointInstance();
    }

    #endregion
}
