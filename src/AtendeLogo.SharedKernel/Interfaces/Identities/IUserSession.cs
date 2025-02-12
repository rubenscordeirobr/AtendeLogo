namespace AtendeLogo.Shared.Interfaces.Identities;
public interface IUserSession
{
    Guid Id { get; }
    string ApplicationName { get; }
    string ClientSessionToken { get; }
    string IpAddress { get; }
    string UserAgent { get; }
    bool IsActive { get; }
    DateTime StartedAt { get; }
    DateTime? TerminatedAt { get; }
    DateTime LastActivity { get; }
    Language Language { get; }
    AuthenticationType AuthenticationType { get; }
    SessionTerminationReason? TerminationReason { get; }
    GeoLocation? GeoLocation { get; }
    Guid? Tenant_Id { get; }
    Guid User_Id { get; }
    IUser? User { get; }
}
