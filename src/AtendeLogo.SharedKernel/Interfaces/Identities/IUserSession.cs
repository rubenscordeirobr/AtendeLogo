namespace AtendeLogo.Shared.Interfaces.Identities;
public interface IUserSession
{
    Guid Id { get; }
    string ApplicationName { get; }
    string ClientSessionToken { get; }
    string IPAddress { get; }
    string Language { get; }
    string UserAgent { get; }
    bool IsActive { get; }
    DateTime SessionStart { get; }
    DateTime? SessionEnd { get; }
    DateTime? LastActivity { get; }
    GeoLocation? GeoLocation { get; }
    Guid? Tenant_Id { get; }
    Guid User_Id { get; }
    IUser? User { get; }

}
