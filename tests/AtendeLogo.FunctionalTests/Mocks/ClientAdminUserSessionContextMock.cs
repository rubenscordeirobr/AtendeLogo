using AtendeLogo.Shared.Models.Security;
using AtendeLogo.UseCases.Identities.Shared;

namespace AtendeLogo.FunctionalTests.Mocks;

public class ClientAdminUserSessionContextMock : IClientAdminUserSessionContext
{
    private UserSessionResponse? _userSession;
    private UserResponse? _user;
    private UserSessionClaims? _userSessionClaims;

    public bool IsAuthenticated =>
        _userSession is not null &&
        _user is not null &&
        _userSessionClaims is not null;

    public UserSessionClaims? UserSessionClaims
        => _userSessionClaims;

    public UserSessionResponse? UserSession
        => _userSession;

    public UserResponse? User
        => _user;

    public void ClearSessionContext()
    {
        _userSession = null;
        _user = null;
        _userSessionClaims = null;
    }

    public void SetSessionContext(
        UserSessionClaims userSessionClaims,
        UserSessionResponse userSession,
        UserResponse user)
    {
        _userSessionClaims = userSessionClaims;
        _userSession = userSession;
        _user = user;
    }
}
