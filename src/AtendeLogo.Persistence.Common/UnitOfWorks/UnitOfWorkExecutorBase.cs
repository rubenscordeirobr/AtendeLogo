using AtendeLogo.Application.Common;
using AtendeLogo.Application.Contracts.Mediators;
using AtendeLogo.Application.Contracts.Persistence;
using AtendeLogo.Application.Events;
using AtendeLogo.Domain.Primitives.Contracts;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace AtendeLogo.Persistence.Common.UnitOfWorks;

internal abstract class UnitOfWorkExecutorBase
{
    private readonly DbContext _dbContext;
    private readonly IRequestUserSessionService _userSessionService;
    protected readonly ILogger<IUnitOfWork> _logger;
    protected readonly IEventMediator EventMediator;

    public UnitOfWorkExecutorBase(
        DbContext dbContext,
        IRequestUserSessionService userSessionService,
        IEventMediator eventMediator,
        ILogger<IUnitOfWork> logger)
    {
        _dbContext = dbContext;
        _userSessionService = userSessionService;
        _logger = logger;

        EventMediator = eventMediator;
    }

    protected async Task<SaveChangesResult> ExecuteSaveChangesAsync(
        bool silent,
        CancellationToken cancellationToken)
    {
        var entries = _dbContext.ChangeTracker
            .Entries<EntityBase>()
            .Where(x => x.HasChanges())
            .ToList();

        var userSession = _userSessionService.GetCurrentSession();
        var domainEventContext = DomainEventContextFactory.Create(userSession, entries);

        await PreProcessorDispatchAsync(domainEventContext);

        domainEventContext.LockCancellation();

        if (domainEventContext.IsCanceled)
        {
            _logger.LogError("Domain event context is canceled. {Message}.", domainEventContext.Error.Message);
            return SaveChangesResult.DomainEventError(domainEventContext, domainEventContext.Error);
        }

        if (cancellationToken.IsCancellationRequested)
        {
            return SaveChangesResult.OperationCanceledError(cancellationToken, domainEventContext);
        }

        if (cancellationToken.IsCancellationRequested)
        {
            return SaveChangesResult.OperationCanceledError(new OperationCanceledException(cancellationToken), domainEventContext);
        }

        SetCreationAndUpdateDates(userSession, entries);

        try
        {
            var rowAffects = await _dbContext.SaveChangesAsync(cancellationToken);

            await DispatchAsyncDomainEventAsync(domainEventContext, rowAffects);

            return SaveChangesResult.Success(domainEventContext, rowAffects);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during save changes.");
         
            if (!silent)
            {
                throw;
            }
            return SaveChangesResult.SaveChangesError(ex, domainEventContext);
        }
    }

    protected abstract Task PreProcessorDispatchAsync(DomainEventContext domainEventContext);

    protected abstract Task DispatchAsyncDomainEventAsync(
        DomainEventContext domainEventContext,
        int rowAffects);

    public abstract Task<SaveChangesResult> SaveChangesAsync(bool silent, CancellationToken cancellationToken);

    private void SetCreationAndUpdateDates(
        IUserSession userSession,
        List<EntityEntry<EntityBase>> entries)
    {
        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.SetCreateSession(userSession.Id);
                    break;
                case EntityState.Modified:
                    entry.Entity.SetUpdateSession(userSession.Id);
                    break;
                case EntityState.Deleted:

                    if (entry.Entity is ISoftDeletableEntity deletableEntity)
                    {
                        deletableEntity.MarkAsDeleted(userSession);
                        entry.State = EntityState.Modified;
                    }
                    break;
                case EntityState.Detached:
                case EntityState.Unchanged:
                    //do nothing
                    break;
                default:
                    throw new InvalidOperationException($"Invalid state {entry.State} for entity {entry.Entity.GetType().Name}.");
            }
        }
    }
}
