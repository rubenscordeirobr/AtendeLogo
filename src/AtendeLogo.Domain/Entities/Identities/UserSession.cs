using System.Text.Json.Serialization;

namespace AtendeLogo.Domain.Entities.Identities;

public sealed class UserSession : EntityBase, IUserSession, IEventAggregate
{
    private const int UpdateCheckIntervalMinutes = 5;

    private readonly List<IDomainEvent> _events = new();
    public string ClientSessionToken { get; private set; }
    public string ApplicationName { get; private set; }
    public string IpAddress { get; private set; }
    public string? AuthToken { get; private set; }
    public string UserAgent { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime LastActivity { get; private set; }
    public DateTime StartedAt { get; private set; }
    public DateTime? TerminatedAt { get; private set; }
    public TimeSpan? ExpirationTime { get; private set; }
    public Language Language { get; private set; }
    public AuthenticationType AuthenticationType { get; private set; }
    public SessionTerminationReason? TerminationReason { get; private set; }
    public UserRole UserRole { get; private set; }
    public Guid User_Id { get; private set; }
    public User? User { get; private set; }
    public Guid? Tenant_Id { get; private set; }
    public Tenant? Tenant { get; private set; }
    public GeoLocation GeoLocation { get; private set; } = GeoLocation.Empty;
     
    [JsonConstructor]
    internal UserSession(
        string applicationName,
        string clientSessionToken,
        string ipAddress,
        string userAgent,
        AuthenticationType authenticationType,
        Language language,
        UserRole userRole,
        TimeSpan? expirationTime,
        Guid user_Id,
        Guid? tenant_Id)
    {
        ApplicationName = applicationName;
        ClientSessionToken = clientSessionToken;
        IpAddress = ipAddress;
        UserAgent = userAgent;
        AuthenticationType = authenticationType;
        Language = language;
        UserRole = userRole;
        ExpirationTime = expirationTime;
        User_Id = user_Id;
        Tenant_Id = tenant_Id;
        IsActive = true;
    }

    public bool IsUpdatePending()
      => this.IsActive && LastActivity.IsExpired(TimeSpan.FromMinutes(UpdateCheckIntervalMinutes));

    public bool IsExpired()
    {
        if (IsActive && ExpirationTime.HasValue)
        {
            return DateTime.Now > LastActivity.Add(ExpirationTime.Value);
        }
        return false;
    }

    public void TerminateSession(SessionTerminationReason reason)
    {
        if (Id == AnonymousIdentityConstants.Session_Id)
        {
            throw new InvalidOperationException("Anonymous system session cannot be terminated.");
        }

        if (!IsActive)
        {
            throw new InvalidOperationException("Session is already terminated.");
        }

        this.IsActive = false;
        this.TerminatedAt = DateTime.Now;
        this.TerminationReason = reason;
        this._events.Add(new UserSessionTerminatedEvent(this, reason));
    }

    public void UpdateLastActivity()
    {
        this.LastUpdatedAt = DateTime.UtcNow;
    }

    #region IUser, IDomainEventAggregate

    IUser? IUserSession.User => User;

    public IReadOnlyList<IDomainEvent> DomainEvents => _events;

    #endregion

}
