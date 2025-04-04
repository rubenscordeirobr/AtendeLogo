using AtendeLogo.Application.Abstractions.Handlers;

namespace AtendeLogo.Application.Abstractions.Events;

public interface IDomainEventHandler<in TEvent> : IApplicationHandler
    where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent domainEvent);

    Task IApplicationHandler.HandleAsync(object handlerObject)
    {
        return HandleAsync((TEvent)handlerObject);
    }
}
