namespace AtendeLogo.Domain.Entities.Identities;

public sealed class UserSession : EntityBase, IUserSession, IEventAggregate
{
    private const int UpdateCheckIntervalMinutes  = 5;

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
    public Language Language { get; private set; }
    public AuthenticationType AuthenticationType { get; private set; }
    public SessionTerminationReason? TerminationReason { get;private set; }
    public Guid User_Id { get; private set; }
    public User? User { get; private set; }
    public Guid? Tenant_Id { get; private set; }
    public Tenant? Tenant { get; private set; }
    public GeoLocation GeoLocation { get; private set; } = GeoLocation.Empty;
      
    public UserSession(
        string applicationName,
        string clientSessionToken,
        string ipAddress,
        string userAgent,
        string? authToken,
        Language language,
        AuthenticationType authenticationType,
        Guid user_Id,
        Guid? tenant_Id)
    {
        ApplicationName = applicationName;
        IpAddress = ipAddress;
        UserAgent = userAgent;
        Language = language;
        AuthToken = authToken;
        User_Id = user_Id;
        Tenant_Id = tenant_Id;
        ClientSessionToken = clientSessionToken;
        AuthenticationType = authenticationType;
        IsActive = true;
    }

    public bool IsUpdatePending()
      => this.IsActive && LastActivity.IsExpired(TimeSpan.FromMinutes(UpdateCheckIntervalMinutes));

    public void TerminateSession(SessionTerminationReason reason)
    {
        if (Id == AnonymousConstants.AnonymousSystemSession_Id)
        {
            throw new InvalidOperationException("Anonymous system session cannot be terminated.");
        }

        this.IsActive = false;
        this.TerminatedAt = DateTime.Now;
        this.TerminationReason = reason;
        this._events.Add(new UserSessionTerminatedEvent(this, reason));
    }

    public void SetAnonymousSystemSessionId()
    {
        if(Id!= default)
        {
            throw new InvalidOperationException("Id is already set.");
        }

        SetCreateSession(AnonymousConstants.AnonymousSystemSession_Id);

        Id = AnonymousConstants.AnonymousSystemSession_Id;
        AuthenticationType = AuthenticationType.Anonymous;
        AuthToken = null;
        Tenant_Id = null;
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
