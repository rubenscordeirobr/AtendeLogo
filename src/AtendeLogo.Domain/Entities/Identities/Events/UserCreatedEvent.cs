namespace AtendeLogo.Domain.Entities.Identities.Events;

public record UserCreatedEvent : IDomainEvent
{
    public User UserCreated { get; }

    public UserCreatedEvent(User userCreated)
    {
        UserCreated = userCreated;
    }
}
