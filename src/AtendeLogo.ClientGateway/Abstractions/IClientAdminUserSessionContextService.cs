using AtendeLogo.Shared.Models.Security;

namespace AtendeLogo.ClientGateway.Abstractions;

public interface IClientAdminUserSessionContextService : IClientUserSessionContextService
{
    AdminUserSessionContext? SessionContext { get; }
    Task<AdminUserSessionContext?> GetSessionContextAsync(); 

    Task SetSessionContextAsync(AdminUserSessionContext sessionContext, bool isPersistent);
}

public sealed record AdminUserSessionContext(
        UserSessionClaims UserSessionClaims,
        UserSessionResponse UserSession,
        UserResponse user);
