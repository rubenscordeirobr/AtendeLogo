using AtendeLogo.Application.Events;

namespace AtendeLogo.Application.Abstractions.Events;

public interface IDomainEventContext
{
    IReadOnlyList<IDomainEvent> Events { get; }

    bool IsCanceled { get; }
    DomainEventError? Error { get; }

    void AddExecutedEventResults(IDomainEvent domainEvent, IReadOnlyList<ExecutedDomainEventResult> results);
    void Cancel(DomainEventError error);
}

