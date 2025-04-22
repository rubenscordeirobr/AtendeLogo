using AtendeLogo.Shared.Models.Security;

namespace AtendeLogo.ClientGateway.Abstractions;

public interface IClientAdminUserSessionContext : IClientUserSessionContext
{
    
    void SetSessionContext(
        UserSessionClaims userSessionClaims,
        UserSessionResponse userSession,
        UserResponse user);
}
