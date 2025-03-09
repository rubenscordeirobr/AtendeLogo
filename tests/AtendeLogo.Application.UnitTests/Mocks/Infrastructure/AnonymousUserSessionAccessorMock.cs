using AtendeLogo.Common.Infos;
using AtendeLogo.Shared.Contantes;
using AtendeLogo.Shared.Interfaces.Identities;
using AtendeLogo.Persistence.Identity.Extensions;
using AtendeLogo.Common.Helpers;
using AtendeLogo.Shared.Contracts;

namespace AtendeLogo.Application.UnitTests.Mocks.Infrastructure;

public abstract class UserSessionAccessorMock : IUserSessionAccessor
{
    public void AddClientSessionCookie(string session)
    {
        // Do nothing        
    }

    public void RemoveClientSessionCookie(string clientSessionToken)
    {
        // Do nothing
    }

    public abstract UserSession GetCurrentSession();


    public string? GetClientSessionToken()
    {
        return null;
    }

    IUserSession IUserSessionAccessor.GetCurrentSession()
    {
        return GetCurrentSession();
    }

    public ClientRequestHeaderInfo GetClientRequestHeaderInfo()
    {
        return new ClientRequestHeaderInfo("localhost", "Tests", "Tests");
    }

    public Type? GetCurrentEndpointType()
        => typeof(UserAuthenticationServiceMock);

    public IEndpointService? GetCurrentEndpointInstance()
        =>  new UserAuthenticationServiceMock();
}

public class UserAuthenticationServiceMock : IEndpointService
{
    public ServiceRole ServiceRole
        => ServiceRole.UserAuthentication;

    public string ServiceName
        => "UserAuthentication";


    public bool IsAllowAnonymous
        => true;
}

public class AnonymousUserSessionAccessorMock : UserSessionAccessorMock
{
    public override UserSession GetCurrentSession()
    {
        var headerInfo = ClientRequestHeaderInfo.Unknown;
        var clientSessionToken = AnonymousIdentityConstants.ClientAnonymousSystemSessionToken;

        var userSession = new UserSession(
            applicationName: "AtendeLogo",
            clientSessionToken: clientSessionToken,
            ipAddress: headerInfo.IpAddress,
            userAgent: "SYSTEM",
            language: Language.Default,
            authenticationType: AuthenticationType.Anonymous,
            userRole: UserRole.None,
            user_Id: AnonymousIdentityConstants.AnonymousUser_Id,
            expirationTime: null,
            tenant_Id: null
        );
        userSession.SetAnonymousSystemSessionId();
        return userSession;
    }
}
public class SystemTenantUserSessionAccessorMock : UserSessionAccessorMock
{
    public override UserSession GetCurrentSession()
    {
        var headerInfo = ClientRequestHeaderInfo.Unknown;
        var clientSessionToken = HashHelper.CreateSha256Hash(Guid.NewGuid());

        var userSession = new UserSession(
            applicationName: "AtendeLogo",
            clientSessionToken: clientSessionToken,
            ipAddress: headerInfo.IpAddress,
            userAgent: "SYSTEM",
            language: Language.Default,
            authenticationType: AuthenticationType.System,
            userRole: UserRole.Owner,
            expirationTime: null,
            user_Id: SystemTenantConstants.TenantSystemOwnerUser_Id,
            tenant_Id: SystemTenantConstants.TenantSystem_Id
        );
        userSession.SetPropertyValue(x => x.Id, Guid.NewGuid());
        return userSession;
    }
}
