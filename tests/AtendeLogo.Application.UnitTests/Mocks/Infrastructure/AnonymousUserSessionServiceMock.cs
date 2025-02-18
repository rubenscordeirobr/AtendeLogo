using AtendeLogo.Common.Infos;
using AtendeLogo.Shared.Contantes;

namespace AtendeLogo.Application.UnitTests.Mocks.Infrastructure;
internal class AnonymousUserSessionServiceMock : IUserSessionService
{
    public UserSession GetCurrentSession()
    {
        var headerInfo = RequestHeaderInfo.Unknown;
        var clientSessionToken = AnonymousConstants.ClientAnymousSystemSessionToken;

        var userSession = new UserSession(
            applicationName: "AtendeLogo",
            clientSessionToken: clientSessionToken,
            ipAddress: headerInfo.IpAddress,
            userAgent: "SYSTEM",
            language: Language.Default,
            authenticationType: AuthenticationType.Anonymous,
            user_Id: AnonymousConstants.AnonymousUser_Id,
            authToken: null,
            tenant_Id: null
        );

        userSession.SetAnonymousSystemSessionId();
        return userSession;
    }
}
