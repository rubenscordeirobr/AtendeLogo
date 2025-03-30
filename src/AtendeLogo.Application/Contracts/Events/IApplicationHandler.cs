using AtendeLogo.Application.Contracts.Handlers;

namespace AtendeLogo.Application.Contracts.Events;

public interface IDomainEventHandler<in TEvent> : IApplicationHandler
    where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent domainEvent);

    Task IApplicationHandler.HandleAsync(object handlerObject)
    {
        return HandleAsync((TEvent)handlerObject);
    }
}
