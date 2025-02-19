namespace AtendeLogo.Application.Models.Identities;


public class AnonymousUserSession : IUserSession
{
    public Guid Id
        => AnonymousConstants.AnonymousSystemSession_Id;
    public Guid User_Id
        => AnonymousConstants.AnonymousUser_Id;
    public string ClientSessionToken
        => AnonymousConstants.ClientAnymousSystemSessionToken;
  
    public required string ApplicationName { get; init; }
    public required string IpAddress { get; init; }
    public required string UserAgent { get; init; }

    public bool IsActive { get; } = true;
    public DateTime StartedAt { get; } = DateTime.UtcNow;
    public DateTime LastActivity => DateTime.UtcNow;
    public DateTime? TerminatedAt { get; } = null;
    public SessionTerminationReason? TerminationReason { get; } = null;
    public Language Language { get; } = Language.Default;
    public AuthenticationType AuthenticationType { get; } = AuthenticationType.Anonymous;
    public GeoLocation? GeoLocation { get; } = null;
    public Guid? Tenant_Id { get; } = null;
    public IUser? User { get; } = null;
}
