namespace AtendeLogo.Domain.Entities.Identities.Events;

public record UserCreatedEvent : DomainEvent
{
    public User UserCreated { get; }

    public UserCreatedEvent(User userCreated)
    {
        UserCreated = userCreated;
    }
}

public class DomainEvent<TEntity> : IDomainEvent
    where TEntity : EntityBase
{
}
