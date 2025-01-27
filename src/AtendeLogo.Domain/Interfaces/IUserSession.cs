namespace AtendeLogo.Domain.Interfaces;
public interface IUserSession
{
    Guid Id { get; }
    Guid ClientSessionId { get; }
    Guid UserId { get; }
    Guid? TenantId { get; }
    DateTime SessionStart { get; }
    DateTime? SessionEnd { get; }
    DateTime? LastActivity { get; }
    string IPAddress { get; }
    string ApplicationName { get; }
    string UserAgent { get; }
    string Language { get; }
    GeoLocation? GeoLocation { get; }
    bool IsActive { get; }
}

