using AtendeLogo.Common.Infos;
using AtendeLogo.Persistence.Identity.Extensions;
using AtendeLogo.Common.Helpers;

namespace AtendeLogo.TestCommon.Mocks.Infrastructure;

public abstract class UserSessionAccessorMock : IUserSessionAccessor
{
    private UserSession? CurrentSession
        => field ??= GetCurrentSession();

    public void AddClientSessionCookie(string session)
    {
        // Do nothing        
    }
     
    public void RemoveClientSessionCookie(string clientSessionToken)
    {
        // Do nothing
    }

    protected abstract UserSession GetCurrentSession();

    public string? GetClientSessionToken()
    {
        return CurrentSession?.ClientSessionToken;
    }

    IUserSession IUserSessionAccessor.GetCurrentSession()
    {
        return CurrentSession!;
    }

    public ClientRequestHeaderInfo GetClientRequestHeaderInfo()
    {
        var currentSession = CurrentSession!;
        return new ClientRequestHeaderInfo(
            currentSession.IpAddress, 
            currentSession.UserAgent,
            currentSession.ApplicationName);
    }
     
    public IEndpointService? GetCurrentEndpointInstance()
        => new UserAuthenticationServiceMock();
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
    protected override UserSession GetCurrentSession()
    {
        var headerInfo = ClientRequestHeaderInfo.System;
        var clientSessionToken = AnonymousIdentityConstants.ClientSystemSessionToken;

        var userSession = new UserSession(
            applicationName: "AtendeLogo",
            clientSessionToken: clientSessionToken,
            ipAddress: headerInfo.IpAddress,
            userAgent: headerInfo.UserAgent,
            language: Language.Default,
            authenticationType: AuthenticationType.Anonymous,
            userRole: UserRole.Anonymous,
            userType: UserType.Anonymous,
            user_Id: AnonymousIdentityConstants.User_Id,
            expirationTime: null,
            tenant_Id: null
        );
        userSession.SetAnonymousSystemSessionId();
        return userSession;
    }
}
public class TenantOwnerUserSessionAccessorMock : UserSessionAccessorMock
{
    protected override UserSession GetCurrentSession()
    {
        var headerInfo = ClientRequestHeaderInfo.Unknown;
        var clientSessionToken = HashHelper.CreateSha256Hash(SystemTenantConstants.Tenant_Id);

        var userSession = new UserSession(
            applicationName: "AtendeLogo",
            clientSessionToken: clientSessionToken,
            ipAddress: headerInfo.IpAddress,
            userAgent: "SYSTEM",
            language: Language.Default,
            authenticationType: AuthenticationType.System,
            userRole: UserRole.Owner,
            userType: UserType.TenantUser,
            expirationTime: null,
            user_Id: SystemTenantConstants.OwnerUser_Id,
            tenant_Id: SystemTenantConstants.Tenant_Id
        );
        userSession.SetPropertyValue(x => x.Id, Guid.NewGuid());
        return userSession;
    }
}
public class SystemAdminUserSessionAccessorMock : UserSessionAccessorMock
{
    private static Guid RandomGuid = new Guid("d41b2318-bcd5-42ea-ae4d-31ad0b69ed22");
    protected override UserSession GetCurrentSession()
    {
        var headerInfo = ClientRequestHeaderInfo.Unknown;
        var clientSessionToken = HashHelper.CreateSha256Hash(RandomGuid);

        var userSession = new UserSession(
            applicationName: "AtendeLogo",
            clientSessionToken: clientSessionToken,
            ipAddress: headerInfo.IpAddress,
            userAgent: "SYSTEM",
            language: Language.Default,
            authenticationType: AuthenticationType.System,
            userRole: UserRole.SystemAdmin,
            userType: UserType.SystemUser,
            expirationTime: null,
            user_Id: Guid.NewGuid(),
            tenant_Id: null
        );
        userSession.SetPropertyValue(x => x.Id, Guid.NewGuid());
        return userSession;
    }
}
