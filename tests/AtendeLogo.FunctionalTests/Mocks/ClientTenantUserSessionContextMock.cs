using AtendeLogo.Shared.Models.Security;
using AtendeLogo.UseCases.Identities.Shared;

namespace AtendeLogo.FunctionalTests.Mocks;

public class ClientTenantUserSessionContextMock : IClientTenantUserSessionContext
{
    private UserSessionResponse? _userSession;
    private UserResponse? _user;
    private TenantResponse? _tenant;
    private UserSessionClaims? _userSessionClaims;
     
    public bool IsAuthenticated =>
        _userSession is not null &&
        _user is not null &&
        _tenant is not null &&
        _userSessionClaims is not null;

    public UserSessionClaims? UserSessionClaims
        => _userSessionClaims;

    public UserSessionResponse? UserSession
        => _userSession;

    public UserResponse? User
        => _user;

    public TenantResponse? Tenant
        => _tenant;

    public void ClearSessionContext()
    {
        _userSession = null;
        _user = null;
        _tenant = null;
        _userSessionClaims = null;
    }

    public void SetSessionContext(
        UserSessionClaims userSessionClaims,
        UserSessionResponse userSession,
        UserResponse user,
        TenantResponse tenant)
    {
        _userSessionClaims = userSessionClaims;
        _userSession = userSession;
        _user = user;
        _tenant = tenant;
    }
     
}
