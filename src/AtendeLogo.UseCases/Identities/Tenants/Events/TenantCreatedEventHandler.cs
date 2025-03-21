using AtendeLogo.Domain.Entities.Identities.Events;

namespace AtendeLogo.UseCases.Identities.Tenants.Events;

internal class TenantCreatedEventHandler : IDomainEventHandler<TenantCreatedEvent>
{
    private readonly ILogger<TenantCreatedEventHandler> _logger;
 
    public TenantCreatedEventHandler(
        ILogger<TenantCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(TenantCreatedEvent domainEvent)
    {
        _logger.LogInformation("Tenant created: {Tenant}", domainEvent.Tenant);

        //TODO : Send email to owner and Create user session for owner to avoid login again
        return Task.CompletedTask;
    }
}
