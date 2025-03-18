using AtendeLogo.Persistence.Common.Exceptions;

namespace AtendeLogo.Persistence.Common.UnitOfWorks;

public abstract partial class UnitOfWork<TDbContext> : IUnitOfWork, IAsyncDisposable
    where TDbContext : DbContext
{
    protected readonly TDbContext _dbContext;
    protected readonly IUserSessionAccessor _userSessionAccessor;

    private readonly IEntityAuthorizationService _entityAuthorizationService;
    private readonly IEventMediator _eventMediator;
    private readonly ILogger<IUnitOfWork> _logger;

    private TransactionUnitOfWorkExecutor? _transactionExecutor;
    private readonly SemaphoreSlim _lock = new(1, 1);

    public UnitOfWork(
        TDbContext dbContext,
        IUserSessionAccessor userSessionService,
        IEntityAuthorizationService entityAuthorizationService,
        IEventMediator eventMediator,
        ILogger<IUnitOfWork> logger)
    {
        Guard.NotNull(dbContext);
        Guard.NotNull(userSessionService);

        _dbContext = dbContext;
        _userSessionAccessor = userSessionService;

        _entityAuthorizationService = entityAuthorizationService;
        _eventMediator = eventMediator;
        _logger = logger;
 
        if (dbContext.ChangeTracker.HasChanges())
        {
            throw new InvalidOperationException("There are tracked entities. Use SaveChangesAsync for this operation.");
        }
    }

    #region Commands

    public void Add<TEntity>(TEntity entity) where TEntity : EntityBase
    {
        if (entity.Id != default)
        {
            throw new InvalidOperationException("Entity already has an id.");
        }
        _dbContext.Set<TEntity>().Add(entity);
    }

    public void Update<TEntity>(TEntity entity) where TEntity : EntityBase
    {
        VerifyEntityIsTracked(entity);
        _dbContext.Set<TEntity>().Update(entity);
    }

    public void Delete<TEntity>(TEntity entity) where TEntity : EntityBase
    {
        VerifyEntityIsTracked(entity);
        _dbContext.Set<TEntity>().Remove(entity);
    }

    public void Attach<TEntity>(TEntity entity) where TEntity : EntityBase
    {
        var entry = _dbContext.Entry(entity);
        if (entry.State == EntityState.Detached)
        {
            _dbContext.Set<TEntity>().Attach(entity);
        }
    }

    protected T LazyInitialize<T>(ref T? repository, Func<T> factory) where T : class
    {
        return repository ??= factory();
    }

    #endregion

    public async Task<SaveChangesResult> SaveChangesAsync(
       CancellationToken cancellationToken = default)
    {
        return await SaveChangesAsync(silent: false, cancellationToken);
    }
 
    public async Task<SaveChangesResult> SaveChangesAsync(
        bool silent,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _lock.WaitAsync(cancellationToken);

            if (_transactionExecutor != null)
            {
                return await _transactionExecutor.SaveChangesAsync(silent, cancellationToken);
            }

            var executor = new UnitOfWorkExecutor(
                _dbContext,
                _userSessionAccessor,
                _entityAuthorizationService,
                _eventMediator,
                _logger);

            return await executor.SaveChangesAsync(silent, cancellationToken);
        }
        finally
        {
            _lock.Release();
        }
    }

    #region Transaction

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_dbContext.Database.IsInMemory())
            return;

        if (_transactionExecutor != null)
            throw new InvalidOperationException("Failed to begin transaction. There is already an open transaction.");

        try
        {
            _lock.Wait(cancellationToken);

            var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
           
            _transactionExecutor = new TransactionUnitOfWorkExecutor(
                _dbContext,
                _userSessionAccessor,
                _entityAuthorizationService,
                _eventMediator,
                _logger,
                transaction);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<SaveChangesResult> CommitAsync(
        CancellationToken cancellationToken = default)
    {
        return await CommitAsync(silent: false, cancellationToken);
    }

    public async Task<SaveChangesResult> CommitAsync(
        bool silent,
        CancellationToken cancellationToken = default)
    {
        if (_dbContext.Database.IsInMemory())
            return await SaveChangesAsync(silent, cancellationToken);

        if (_transactionExecutor == null)
            throw new InvalidOperationException("There is no active transaction to commit.");
     
        try
        {
            _lock.Wait(cancellationToken);

            var result = await _transactionExecutor.CommitAsync(silent, cancellationToken);
            _transactionExecutor = null;
            return result;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task RollbackAsync(
        Exception exception,
        CancellationToken cancellationToken = default)
    {

        if (_dbContext.Database.IsInMemory())
            return;

        if (_transactionExecutor == null)
            throw new InvalidOperationException("Failed to rollback transaction. There is no open transaction.");

        try
        {
            _lock.Wait(cancellationToken);

            await _transactionExecutor.TryRollbackAsync(exception, cancellationToken);
            _transactionExecutor = null;
        }
        finally
        {
            _lock.Release();
        }
    }

    #endregion
     
    public async virtual ValueTask DisposeAsync()
    {
        if (_transactionExecutor != null)
        {
            await HandleTransactionOpenExceptionAsync();
        }
    }

    private async Task HandleTransactionOpenExceptionAsync()
    {
        Guard.NotNull(_transactionExecutor);

        Exception? rollbackInnerException = null;
        try
        {
            await _transactionExecutor.DisposeAsync();
        }
        catch (Exception ex)
        {
            rollbackInnerException = ex;
        }

        _transactionExecutor = null;

        throw new TransactionOpenException(
            "Failed to dispose unit of work. There is an open transaction. Rollback was invoked",
            rollbackInnerException);
    }

    private void VerifyEntityIsTracked<TEntity>(TEntity entity) where TEntity : EntityBase
    {
        var entry = _dbContext.Entry(entity);
        if (entry.State == EntityState.Detached)
        {
            throw new InvalidOperationException($"Entity {entity} is not tracked. Make sure to get entity from DbContext with the tracking option enabled.");
        }
    }
}
