using AtendeLogo.Domain.Domain;

namespace AtendeLogo.Domain.Entities.Identities;
public class UserSession : EntityBase, IUserSession
{
    public Guid ClientSessionId { get; private set; }
    public DateTime SessionStart { get; private set; }
    public DateTime? SessionEnd { get; private set; }
    public DateTime? LastActivity { get; private set; }
    public string IPAddress { get; private set; }
    public string ApplicationName { get; private set; }
    public string UserAgent { get; private set; }
    public string Language { get; private set; }
    public bool IsActive { get; private set; }
    public GeoLocation? GeoLocation { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; }
    public Guid? TenantId { get; private set; }
    public Tenant? Tenant { get; private set; }
    public AuthenticationType AuthenticationType { get; private set; }

    public UserSession(
        AuthenticationType authenticationType,
        Guid clientSessionId,
        DateTime sessionStart,
        DateTime? sessionEnd,
        DateTime? lastActivity,
        string ipAddress,
        string applicationName,
        GeoLocation? geoLocation,
        Guid userId,
        User user,
        Guid? tenantId,
        Tenant? tenant,
        string userAgent,
        string language,
        bool isActive)
    {
        AuthenticationType = authenticationType;
        ClientSessionId = clientSessionId;
        SessionStart = sessionStart;
        SessionEnd = sessionEnd;
        LastActivity = lastActivity;
        IPAddress = ipAddress;
        ApplicationName = applicationName;
        GeoLocation = geoLocation;
        UserId = userId;
        User = user;
        TenantId = tenantId;
        Tenant = tenant;
        UserAgent = userAgent;
        Language = language;
        IsActive = isActive;
    }
    
}
