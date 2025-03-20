﻿using AtendeLogo.Common.Infos;
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
        var clientSessionToken = AnonymousIdentityConstants.ClientSystemSessionToken;

        var userSession = new UserSession(
            applicationName: "AtendeLogo",
            clientSessionToken: clientSessionToken,
            ipAddress: headerInfo.IpAddress,
            userAgent: "SYSTEM",
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
