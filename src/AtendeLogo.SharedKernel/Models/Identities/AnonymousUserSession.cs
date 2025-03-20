using AtendeLogo.Shared.Constants;
using AtendeLogo.Shared.Interfaces.Identities;

namespace AtendeLogo.Shared.Models.Identities;

public class AnonymousUserSession : IUserSession
{
    public Guid Id
        => AnonymousIdentityConstants.Session_Id;
    public Guid User_Id
        => AnonymousIdentityConstants.User_Id;
    public string ClientSessionToken
        => AnonymousIdentityConstants.ClientSystemSessionToken;
  
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
    public UserRole UserRole { get; } = UserRole.Anonymous;
    public UserType UserType { get; } = UserType.Anonymous;
    public GeoLocation? GeoLocation { get; } = null;
    public Guid? Tenant_Id { get; } = null;
    public IUser? User { get; } = null;
}
