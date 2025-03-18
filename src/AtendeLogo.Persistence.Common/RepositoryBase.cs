using System.Linq.Expressions;
using AtendeLogo.Application.Contracts.Persistence;
using AtendeLogo.Common;
using AtendeLogo.Domain.Primitives.Contracts;
using AtendeLogo.Persistence.Common.Enums;
using AtendeLogo.Persistence.Common.Exceptions;

namespace AtendeLogo.Persistence.Common;

public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : EntityBase
{
    private readonly DbContext _dbContext;
    protected readonly IUserSessionAccessor _userSessionAccessor;
    protected readonly TrackingOption _trackingOption;

    private readonly bool _isImplementDeletedInterface;
    private readonly bool _isImplementTenantOwnedInterface;

    private bool _isIncludeDeleted = false;

    protected virtual int DefaultMaxRecords { get; } = RepositoryConstants.DefaultMaxRecords;

    public RepositoryBase(
        DbContext dbContext,
        IUserSessionAccessor userSessionAccessor,
        TrackingOption trackingOption)
    {
        Guard.NotNull(dbContext);

        _dbContext = dbContext;
        _isImplementDeletedInterface = typeof(ISoftDeletableEntity).IsAssignableFrom(typeof(TEntity));
        _isImplementTenantOwnedInterface = typeof(ITenantOwned).IsAssignableFrom(typeof(TEntity));
        _trackingOption = trackingOption;
        _userSessionAccessor = userSessionAccessor;
    }

    #region Queries
    public async Task<TEntity?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object?>>[] includes)
    {
        return await CreateQuery(includes)
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
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object?>>[] includeExpressions)
    {
        if (predicate == null)
            throw new ArgumentNullException(nameof(predicate));

        return await CreateQuery(includeExpressions)
            .AnyAsync(predicate, cancellationToken);
    }

    #endregion

    public async Task<TEntity> RefreshAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        Guard.NotNull(entity);

        var refreshedEntity = await TryRefreshAsync(entity, cancellationToken);
        if (refreshedEntity is null)
            throw new EntityNotFoundException(typeof(TEntity), entity!.Id);

        return refreshedEntity;
    }

    public async Task<TEntity?> TryRefreshAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        Guard.NotNull(entity);

        if (entity.Id == default)
            throw new ArgumentException("Is not possible to refresh entity with new entit.");

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
        Expression<Func<TEntity, object?>>[] includeExpressions)
    {
        var query = _dbContext.Set<TEntity>()
              .ApplyTracking(_trackingOption)
              .Take(DefaultMaxRecords);

        if (includeExpressions is not null)
        {
            query = includeExpressions.Aggregate(query, (current, include) => current.Include(include));
        }

        if (_isImplementTenantOwnedInterface)
        {
            if (ShouldFilterTenantOwned())
            {
                var userSession = _userSessionAccessor.GetCurrentSession();

                query = query.Cast<ITenantOwned>()
                   .Where(x => x.Tenant_Id == userSession.Tenant_Id)
                   .Cast<TEntity>();
            }
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

    #endregion
}
