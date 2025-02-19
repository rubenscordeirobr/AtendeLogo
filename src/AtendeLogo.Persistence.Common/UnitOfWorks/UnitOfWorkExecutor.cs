using AtendeLogo.Application.Common;
using AtendeLogo.Application.Contracts.Mediators;
using AtendeLogo.Application.Contracts.Persistence;
using AtendeLogo.Application.Events;
using Microsoft.Extensions.Logging;

namespace AtendeLogo.Persistence.Common.UnitOfWorks;

internal class UnitOfWorkExecutor : UnitOfWorkExecutorBase
{
    public UnitOfWorkExecutor(
        DbContext dbContext,
        IRequestUserSessionService userSessionService,
        IEventMediator eventMediator,
        ILogger<IUnitOfWork> logger)
        : base(dbContext, userSessionService, eventMediator, logger)
    {
    }
    public override Task<SaveChangesResult> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return ExecuteSaveChangesAsync(cancellationToken);
    }

    protected override Task DispatchAsyncDomainEventAsync(
        DomainEventContext domainEventContext,
        int rowAffects )
    {
        return EventMediator.DispatchAsync(domainEventContext);
    }
    protected override Task PreProcessorDispatchAsync(DomainEventContext domainEventContext)
    {
        return EventMediator.PreProcessorDispatchAsync(domainEventContext);
    }
}
