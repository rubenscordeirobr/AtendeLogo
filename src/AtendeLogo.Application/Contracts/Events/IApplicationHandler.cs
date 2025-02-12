using AtendeLogo.Application.Contracts.Handlers;
using AtendeLogo.Domain.Primitives.Contracts;

namespace AtendeLogo.Application.Contracts.Events;

public interface IDomainEventHandler<TEvent> : IApplicationHandler
    where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent domainEvent);

    Task IApplicationHandler.HandleAsync(object domainEvent)
    {
        return HandleAsync((TEvent)domainEvent);
    }
}
