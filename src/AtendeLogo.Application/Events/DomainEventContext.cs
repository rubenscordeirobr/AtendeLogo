using System.Diagnostics.CodeAnalysis;

namespace AtendeLogo.Application.Events;

public sealed class DomainEventContext : IDomainEventContext
{
    private readonly Dictionary<IDomainEvent, List<ExecutedDomainEventResult>> _dispatchedHandlers = [];

    private bool _canBeCanceled = true;
    public IReadOnlyList<IDomainEvent> Events { get; }

    [MemberNotNullWhen(true, nameof(Error))]
    [MemberNotNullWhen(true, nameof(Exception))]
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

    public Exception? Exception
        => IsCanceled
            ? new DomainEventException(Error.Message)
            : null;

    public void AddExecutedEventResults(
        IDomainEvent domainEvent,
        IReadOnlyList<ExecutedDomainEventResult> results)
    {
        _dispatchedHandlers.TryAdd(domainEvent, []);
        if (_dispatchedHandlers.TryGetValue(domainEvent, out var value))
        {
            value.AddRange(results);
        }
        else
        {
            _dispatchedHandlers.Add(domainEvent, [.. results]);
        }
    }

    public IReadOnlyList<ExecutedDomainEventResult> GetExecutedEventResults(IDomainEvent domainEvent)
    {
        if (!_dispatchedHandlers.TryGetValue(domainEvent, out var value))
        {
            return [];
        }
        return value;
    }
}
