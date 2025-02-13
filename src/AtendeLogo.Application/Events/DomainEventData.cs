using AtendeLogo.Domain.Primitives.Contracts;

namespace AtendeLogo.Application.Events;

public sealed class DomainEventData<TEvent> :IDomainEventData<TEvent>
    where TEvent : IDomainEvent
{
    public IDomainEventContext Context { get; }
    public TEvent Event { get; }

    public DomainEventData(IDomainEventContext context, TEvent domainEvent)
    {
        Guard.NotNull(context);
        Guard.NotNull(domainEvent);

        Context = context;
        Event = domainEvent;
    }

    public void Cancel(DomainEventError error)
    {
        this.Context.Cancel(error);
    }

    public void Cancel(string code, string message)
    {
        Cancel(new DomainEventError(code, message));
    }
}
