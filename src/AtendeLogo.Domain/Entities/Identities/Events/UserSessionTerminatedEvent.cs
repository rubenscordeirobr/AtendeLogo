namespace AtendeLogo.Domain.Entities.Identities.Events;

public sealed record UserSessionStartedEvent(
        UserSession UserSession) : IDomainEvent;

public sealed record UserSessionTerminatedEvent(
        UserSession UserSession,
        SessionTerminationReason Reason) : IDomainEvent;
