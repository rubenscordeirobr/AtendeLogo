namespace AtendeLogo.Domain.Entities.Identities.Events;
public class UserSessionTerminatedEvent : IDomainEvent
{
    public UserSession UserSession { get; }

    public SessionTerminationReason Reason { get; }

    public UserSessionTerminatedEvent(
        UserSession userSession,
        SessionTerminationReason reason)
    {
        UserSession = userSession;
        Reason = reason;
    }
}
