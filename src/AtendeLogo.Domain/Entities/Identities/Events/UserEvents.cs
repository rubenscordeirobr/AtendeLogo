namespace AtendeLogo.Domain.Entities.Identities.Events;

public record UserCreatedEvent(User UserCreated) : IDomainEvent;
public record PasswordChangedEvent(User user) : IDomainEvent;
