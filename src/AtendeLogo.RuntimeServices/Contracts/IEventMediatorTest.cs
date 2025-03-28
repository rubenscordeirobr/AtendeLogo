using AtendeLogo.Application.Events;

namespace AtendeLogo.RuntimeServices.Contracts;

internal interface IEventMediatorTest 
{
    IReadOnlyList<IDomainEvent> CapturedEvents { get; }
    IReadOnlyList<ExecutedDomainEventResult> ExecutedPreProcessors { get; }
    IReadOnlyList<ExecutedDomainEventResult> ExecutedDomainEvents { get; }

}
