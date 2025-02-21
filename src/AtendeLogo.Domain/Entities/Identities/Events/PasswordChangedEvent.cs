namespace AtendeLogo.Domain.Entities.Identities.Events;

public record PasswordChangedEvent(User user) : IDomainEvent;
