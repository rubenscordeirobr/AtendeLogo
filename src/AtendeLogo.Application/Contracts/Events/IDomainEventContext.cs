using AtendeLogo.Application.Events;
using AtendeLogo.Domain.Primitives.Contracts;

namespace AtendeLogo.Application.Contracts.Events;

public interface IDomainEventContext
{
    IReadOnlyList<IDomainEvent> Events { get; }

    bool IsCanceled { get; }
    DomainEventError? Error { get; }

    void AddExecutedEventResults(IDomainEvent domainEvent, List<ExecutedDomainEventResult> results);
    void Cancel(DomainEventError error);
}

