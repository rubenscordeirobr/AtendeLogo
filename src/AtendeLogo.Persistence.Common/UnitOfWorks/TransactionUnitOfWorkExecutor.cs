using AtendeLogo.Application.Common;
using AtendeLogo.Application.Contracts.Mediators;
using AtendeLogo.Application.Contracts.Persistence;
using AtendeLogo.Application.Events;
using AtendeLogo.Common;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace AtendeLogo.Persistence.Common.UnitOfWorks;

internal class TransactionUnitOfWorkExecutor : UnitOfWorkExecutorBase, IAsyncDisposable
{
    private readonly TransactionDomainEventManager _transactionDomainEventContext;

    private IDbContextTransaction? _transaction;
    private int _totalRowAffects;

    public TransactionUnitOfWorkExecutor(
        DbContext dbContext,
        IUserSessionAccessor userSessionAccessor,
        IEventMediator eventMediator,
        ILogger<IUnitOfWork> logger,
        IDbContextTransaction transaction)
        : base(dbContext, userSessionAccessor, eventMediator, logger)
    {
        _transaction = transaction;
        _transactionDomainEventContext = new();
    }

    public override async Task<SaveChangesResult> SaveChangesAsync(
        bool silent,
        CancellationToken cancellationToken)
    {
        var result = await ExecuteSaveChangesAsync(silent, cancellationToken);
        if (result.IsSuccess)
        {
            _totalRowAffects += result.AffectedRows;
            return result;
        }

        var executionException = result.Exception;
        await TryRollbackAsync(executionException, cancellationToken);

        if (!silent)
        {
            throw executionException;
        }
        return result;
    }

    protected override Task DispatchAsyncDomainEventAsync(
        DomainEventContext domainEventContext,
        int rowAffects)
    {
        _transactionDomainEventContext.Add(domainEventContext);
        return Task.CompletedTask;
    }

    protected override Task PreProcessorDispatchAsync(DomainEventContext domainEventContext)
    {
        _transactionDomainEventContext.Add(domainEventContext);
        return Task.CompletedTask;
    }

    internal async Task<SaveChangesResult> CommitAsync(
        bool silent,
        CancellationToken cancellationToken)
    {
        Guard.NotNull(_transaction);

        var domainEventContext = _transactionDomainEventContext.GetDomainEventContext();
        try
        {
            await EventMediator.PreProcessorDispatchAsync(domainEventContext, cancellationToken);

            if (domainEventContext.IsCanceled)
            {
                await TryRollbackAsync(domainEventContext.GetException(), cancellationToken);
                return SaveChangesResult.DomainEventError(domainEventContext, domainEventContext.Error);
            }

            if (cancellationToken.IsCancellationRequested)
            {
                await TryRollbackAsync(new OperationCanceledException(cancellationToken), cancellationToken);
                return SaveChangesResult.OperationCanceledError(cancellationToken, domainEventContext);
            }

            var result = await ExecuteSaveChangesAsync(silent, cancellationToken);
            if (!result.IsSuccess)
            {
                await TryRollbackAsync(result.Exception, cancellationToken);
                return result;
            }

            await _transaction.CommitAsync(cancellationToken);

            await EventMediator.DispatchAsync(domainEventContext);

            return SaveChangesResult.Success(domainEventContext, _totalRowAffects);
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex, "Operation canceled during transaction commit.");

            await TryRollbackAsync(ex, cancellationToken);
            if (!silent)
            {
                throw;
            }
            return SaveChangesResult.OperationCanceledError(ex, domainEventContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during transaction commit.");
            await TryRollbackAsync(ex, cancellationToken);
            if (silent)
            {
                return SaveChangesResult.SaveChangesError(ex, domainEventContext);
            }
            throw;
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    internal async Task TryRollbackAsync(
        Exception exception,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (_transaction is null)
                return;

            _logger.LogError(exception, "Rollback transaction due to error.");
            await _transaction.RollbackAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during transaction rollback.");
        }
        finally
        {
            await TryDisposeAsync();
        }
    }

    private async Task TryDisposeAsync()
    {
        try
        {
            if (_transaction is null)
                return;

            await _transaction.DisposeAsync();
            _transaction = null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during transaction dispose.");
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction is not null)
        {
            await TryRollbackAsync(new OperationCanceledException("The transaction excecuted was with oppend transaction."), default);
        }
        _transaction = null;
    }
}
