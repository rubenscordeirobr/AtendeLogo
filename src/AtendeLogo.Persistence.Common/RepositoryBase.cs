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
    private readonly IRequestUserSessionService _userSessionService;

    private readonly bool _isImplementDeletedInterface;
    private readonly bool _isImplementTenantOwnedInterface;
    private readonly TrackingOption _trackingOption;

    private bool _isIncludeDeleted = false;

    protected virtual int DefaultMaxRecords { get; } = RepositoryConstants.DefaultMaxRecords;

    protected IQueryable<TEntity> Query
        => GetInitialQuery();

    public RepositoryBase(
        DbContext dbContext,
        IRequestUserSessionService userSessionService,
        TrackingOption trackingOption)
    {
        Guard.NotNull(dbContext);

        _dbContext = dbContext;
        _isImplementDeletedInterface = typeof(ISoftDeletableEntity).IsAssignableFrom(typeof(TEntity));
        _isImplementTenantOwnedInterface = typeof(ITenantOwned).IsAssignableFrom(typeof(TEntity));
        _trackingOption = trackingOption;
        _userSessionService = userSessionService;
    }

    #region Queries
    public async Task<TEntity?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await Query
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<TEntity?> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        Guard.NotNull(predicate);

        return await Query
            .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> FindAllAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        Guard.NotNull(predicate);

        return await Query
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await Query
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        if (predicate == null)
            throw new ArgumentNullException(nameof(predicate));

        return await Query
            .AnyAsync(predicate, cancellationToken);
    }

    #endregion

    public async Task<TEntity> RefreshAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        Guard.NotNull(entity);

        var refreshedEntity = await TryRefreshAsync(entity, cancellationToken);
        if (refreshedEntity is  null)
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

    private IQueryable<TEntity> GetInitialQuery()
    {
        var query = _dbContext.Set<TEntity>()
              .ApplyTracking(_trackingOption)
              .Take(DefaultMaxRecords);

        if (_isImplementTenantOwnedInterface)
        {
            if (CheckIfNeedFilterTentantOwned())
            {
                var userSession = _userSessionService.GetCurrentSession();

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

    private bool CheckIfNeedFilterTentantOwned()
    {
        var userSession = _userSessionService.GetCurrentSession();
        if (userSession.Tenant_Id == Guid.Empty)
        {
            if (IsUserHasAdminPermision(userSession))
            {
                return false;
            }
            throw new UnauthorizedAccessException("User not have permission to access this resource.");
        }
        return true;
    }

    private bool IsUserHasAdminPermision(IUserSession userSession)
    {
        throw new NotImplementedException();
    }

    #endregion
}
