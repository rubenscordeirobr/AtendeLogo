using AtendeLogo.Application.Common;
using AtendeLogo.Application.Contracts.Mediators;
using AtendeLogo.Application.Contracts.Persistence;
using AtendeLogo.Common;
using AtendeLogo.Persistence.Common.Exceptions;
using Microsoft.Extensions.Logging;

namespace AtendeLogo.Persistence.Common.UnitOfWorks;

public abstract partial class UnitOfWork<TDbContext> : IUnitOfWork, IAsyncDisposable
    where TDbContext : DbContext
{
    protected readonly TDbContext DbContext;
    protected readonly IUserSessionService UserSessionService;
    private readonly IEventMediator _eventMediator;
    private readonly ILogger<IUnitOfWork> _logger;

    private TransactionUnitOfWorkExecutor? _transactionExecutor;

    public UnitOfWork(
        TDbContext dbContext,
        IUserSessionService userSessionService,
        IEventMediator eventMediator,
        ILogger<IUnitOfWork> logger)
    {
        Guard.NotNull(dbContext);
        Guard.NotNull(userSessionService);

        DbContext = dbContext;
        UserSessionService = userSessionService;

        _eventMediator = eventMediator;
        _logger = logger;

        if (dbContext.ChangeTracker.HasChanges())
        {
            throw new InvalidOperationException("There are tracked entities. Use SaveChangesAsync for this operation.");
        }
    }

    #region Transaction

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transactionExecutor != null)
            throw new InvalidOperationException("Failed to begin transaction. There is already an open transaction.");

        var transaction = await DbContext.Database.BeginTransactionAsync(cancellationToken);

        _transactionExecutor = new TransactionUnitOfWorkExecutor(
            DbContext,
            UserSessionService,
            _eventMediator,
            _logger,
            transaction);
    }

    public async Task<SaveChangesResult> CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transactionExecutor == null)
            throw new InvalidOperationException("There is no active transaction to commit.");

        var result = await _transactionExecutor.CommitAsync(cancellationToken);
        _transactionExecutor = null;
        return result;
    }

    public async Task RollbackAsync(
        Exception exception,
        CancellationToken cancellationToken = default)
    {
        if (_transactionExecutor == null)
            throw new InvalidOperationException("Failed to rollback transaction. There is no open transaction.");

        await _transactionExecutor.TryRollbackAsync(exception, cancellationToken);
        _transactionExecutor = null;
    }

    #endregion

    #region Commands

    public void Add<TEntity>(TEntity entity) where TEntity : EntityBase
    {
        if (entity.Id != default)
        {
            throw new InvalidOperationException("Entity already has an id.");
        }
        DbContext.Set<TEntity>().Add(entity);
    }

    public void Update<TEntity>(TEntity entity) where TEntity : EntityBase
    {
        VerifyEntityIsTracked(entity);
        DbContext.Set<TEntity>().Update(entity);
    }

    public void Delete<TEntity>(TEntity entity) where TEntity : EntityBase
    {
        VerifyEntityIsTracked(entity);
        DbContext.Set<TEntity>().Remove(entity);
    }

    public void Attach<TEntity>(TEntity entity) where TEntity : EntityBase
    {
        var entry = DbContext.Entry(entity);
        if (entry.State == EntityState.Detached)
        {
            DbContext.Set<TEntity>().Attach(entity);
        }
    }

    public async Task<SaveChangesResult> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (_transactionExecutor != null)
        {
            return await _transactionExecutor.SaveChangesAsync(cancellationToken);
        }

        var executor = new UnitOfWorkExecutor(
            DbContext,
            UserSessionService,
            _eventMediator,
            _logger);

        return await executor.SaveChangesAsync(cancellationToken);
    }

    protected T LazyInitialize<T>(ref T? repository, Func<T> factory) where T : class
    {
        return repository ??= factory();
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
        var entry = DbContext.Entry(entity);
        if (entry.State == EntityState.Detached)
        {
            throw new InvalidOperationException($"Entity {entity} is not tracked. Make sure to get entity from DbContext with the tracking option enabled.");
        }
    }
}
