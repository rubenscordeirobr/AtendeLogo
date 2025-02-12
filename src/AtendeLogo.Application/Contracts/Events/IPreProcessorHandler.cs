using AtendeLogo.Application.Contracts.Handlers;
using AtendeLogo.Domain.Primitives.Contracts;

namespace AtendeLogo.Application.Contracts.Events;

public interface IPreProcessorHandler<TEvent> :IApplicationHandler
    where TEvent : IDomainEvent
{
    Task PreProcessAsync(
        IDomainEventData<TEvent> eventData,
        CancellationToken cancellationToken = default);

    Task IApplicationHandler.HandleAsync(object handlerObject)
    {
        return PreProcessAsync((IDomainEventData<TEvent>)handlerObject);
    }
}
