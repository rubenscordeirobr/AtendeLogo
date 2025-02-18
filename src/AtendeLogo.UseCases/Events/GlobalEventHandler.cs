using AtendeLogo.Domain.Primitives.Contracts;

namespace AtendeLogo.UseCases.Events;
internal class GlobalEventHandler : IDomainEventHandler<IDomainEvent>
{
    private readonly ILogger<GlobalEventHandler> _logger;
  
    public GlobalEventHandler(ILogger<GlobalEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(IDomainEvent domainEvent)
    {
        //tests propose
        _logger.LogInformation("Handling event {EventName}", domainEvent.GetType().Name);
        return Task.CompletedTask;
    }
}
