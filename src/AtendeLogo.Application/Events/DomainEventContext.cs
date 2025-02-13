using System.Diagnostics.CodeAnalysis;
using AtendeLogo.Domain.Primitives.Contracts;

namespace AtendeLogo.Application.Events;

public sealed class DomainEventContext : IDomainEventContext
{
    private bool _canBeCanceled = true;
    private Dictionary<IDomainEvent, List<ExecutedDomainEventResult>> _dispatchedHandlers = new();
    public IReadOnlyList<IDomainEvent> Events { get; }

    [MemberNotNullWhen(true, nameof(Error))]
    public bool IsCanceled { get; private set; }

    public DomainEventError? Error { get; private set; }

    public DomainEventContext(IEnumerable<IDomainEvent> events)
    {
        Guard.NotNull(events);
        Events = events.ToList();
    }

    public void Cancel(DomainEventError error)
    {
        if (!_canBeCanceled)
        {
            throw new InvalidOperationException("The context cannot be canceled.");
        }

        Guard.NotNull(error);
        IsCanceled = true;
        Error = error;
    }

    public void LockCancellation()
    {
        _canBeCanceled = false;
    }

    public Exception GetException()
    {
        if (!IsCanceled)
        {
            throw new InvalidOperationException("The context is not canceled.");
        }
        return new Exception(Error?.Message);
    }

    public void AddExecutedEventResults(IDomainEvent domainEvent, List<ExecutedDomainEventResult> results)
    {
        if (_dispatchedHandlers.ContainsKey(domainEvent))
        {
            _dispatchedHandlers[domainEvent].AddRange(results);
        }
        else
        {
            _dispatchedHandlers.Add(domainEvent, results);
        }
    }

    public IReadOnlyList<ExecutedDomainEventResult> GetExecutedEventResults(IDomainEvent domainEvent)
    {
        if (!_dispatchedHandlers.ContainsKey(domainEvent))
        {
            return Array.Empty<ExecutedDomainEventResult>();
        }
        return _dispatchedHandlers[domainEvent];
    }
}
