using AtendeLogo.Common.Infos;
using AtendeLogo.Shared.Contantes;
using AtendeLogo.Shared.Interfaces.Identities;
using AtendeLogo.Persistence.Identity.Extensions;
using AtendeLogo.Common.Helpers;

namespace AtendeLogo.Application.UnitTests.Mocks.Infrastructure;

public abstract class UserSessionServiceMock : IRequestUserSessionService
{
    public void AddClientSessionCookie(string session)
    {
        // Do nothing        
    }

    public abstract UserSession GetCurrentSession();
   

    public string? GetClientSessionToken()
    {
        return null;
    }

    IUserSession IRequestUserSessionService.GetCurrentSession()
    {
        return GetCurrentSession();
    }

    public RequestHeaderInfo GetRequestHeaderInfo()
    {
        return new RequestHeaderInfo("localhost", "Tests", "Tests");
    }
}

public class AnonymousUserSessionServiceMock : UserSessionServiceMock
{
    public override UserSession GetCurrentSession()
    {
        var headerInfo = RequestHeaderInfo.Unknown;
        var clientSessionToken = AnonymousConstants.ClientAnonymousSystemSessionToken;

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
public class SystemTenantUserSessionServiceMock : UserSessionServiceMock
{
    public override UserSession GetCurrentSession()
    {
        var headerInfo = RequestHeaderInfo.Unknown;
        var clientSessionToken = HashHelper.CreateSha256Hash(Guid.NewGuid());

        var userSession = new UserSession(
            applicationName: "AtendeLogo",
            clientSessionToken: clientSessionToken,
            ipAddress: headerInfo.IpAddress,
            userAgent: "SYSTEM",
            language: Language.Default,
            authenticationType: AuthenticationType.System,
            user_Id: SystemTenantConstants.TenantSystemOwnerUser_Id,
            authToken: clientSessionToken,
            tenant_Id: SystemTenantConstants.TenantSystem_Id
        );
        userSession.SetPropertyValue(x=> x.Id,  Guid.NewGuid());
        return userSession;
    }
}
