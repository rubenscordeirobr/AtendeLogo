using AtendeLogo.Common.Helpers;
using AtendeLogo.Common.Infos;
using AtendeLogo.Domain.Entities.Identities.Factories;
using AtendeLogo.Persistence.Identity.Extensions;
using AtendeLogo.RuntimeServices.Services;
using AtendeLogo.Shared.Factories;
using AtendeLogo.Shared.Models.Security;

namespace AtendeLogo.TestCommon.Mocks;

public abstract class HttpContextSessionAccessorMock : IHttpContextSessionAccessor
{
    private readonly IUserSessionTokenHandler _tokenHandler;

    public string? AuthorizationToken { get; set; }
    public IUserSession? UserSession { get; set; }
    public IEndpointService? EndpointInstance { get; set; }
    public UserSessionClaims? UserSessionClaims { get; set; }
    public ClientRequestHeaderInfo RequestHeaderInfo { get; }

    protected HttpContextSessionAccessorMock(IUserSessionTokenHandler tokenHandler)
    {
        _tokenHandler = tokenHandler;

#pragma warning disable CA2214
        UserSession = CreateMockUserSession();
        UserSessionClaims = UserSessionClaimsFactory.Create(UserSession, GetUser());
#pragma warning restore CA2214 

        RequestHeaderInfo = CreateRequestHeaderInfo();
        AuthorizationToken = RequestHeaderInfo.AuthorizationToken;
        EndpointInstance = new UserAuthenticationServiceMock();
    }
     

    protected abstract IUserSession CreateMockUserSession();
    protected abstract IUser GetUser();

    private ClientRequestHeaderInfo CreateRequestHeaderInfo()
    {
        var token = _tokenHandler.WriteToken(UserSessionClaims!, UserSession!.KeepSession);
        return new ClientRequestHeaderInfo(
            UserSession.IpAddress,
            UserSession.UserAgent,
            UserSession.ApplicationName,
            token);
    }
}

public class UserAuthenticationServiceMock : IEndpointService
{
    public ServiceRole ServiceRole
        => ServiceRole.Authentication;

    public string ServiceName
        => "UserAuthentication";

    public bool IsAllowAnonymous
        => true;
}

public class AnonymousUserSessionAccessorMock : HttpContextSessionAccessorMock
{
    public AnonymousUserSessionAccessorMock(IUserSessionTokenHandler tokenHandler)
        : base(tokenHandler)
    {
    }

    public static IHttpContextSessionAccessor CreateMock(ITestOutputHelper testOutput)
    {
        var logger = new TestOutputLogger<UserSessionTokenHandler>(testOutput);
        var secureConfiguration = new SecureConfigurationMock();
        var tokenHandler = new UserSessionTokenHandler(secureConfiguration, logger);
        return new AnonymousUserSessionAccessorMock(tokenHandler);
    }

    protected override IUserSession CreateMockUserSession()
    {
        return AnonymousUserConstants.AnonymousUserSession;
    }
    protected override IUser GetUser()
    {
        return AnonymousUserConstants.AnonymousUser;
    }
}

public class TenantOwnerUserSessionAccessorMock : HttpContextSessionAccessorMock
{
    public TenantOwnerUserSessionAccessorMock(IUserSessionTokenHandler tokenHandler)
        : base(tokenHandler)
    {
    }
    protected override IUserSession CreateMockUserSession()
    {
        var headerInfo = ClientRequestHeaderInfo.Unknown;
        var ownerUser = SystemTenantConstants.OwnerUser;
        var userSession = UserSessionFactory.Create(
            ownerUser,
            headerInfo,
            AuthenticationType.System,
            keepSession: true,
            SystemTenantConstants.Tenant_Id);

        userSession.SetPropertyValue(x => x.Id, GuidHelper.NewGuidZeroPrefixed());
        return userSession;
    }
    protected override IUser GetUser()
    {
        return SystemTenantConstants.OwnerUser;
    }
}
public class AminUserSessionAccessorMock : HttpContextSessionAccessorMock
{
    public AminUserSessionAccessorMock(IUserSessionTokenHandler tokenHandler)
        : base(tokenHandler)
    {
    }

    protected override IUserSession CreateMockUserSession()
    {
        var headerInfo = ClientRequestHeaderInfo.System;
        var superAdminUser = DefaultAdminUserConstants.User;

        var userSession = UserSessionFactory.Create(
            user: superAdminUser,
            clientHeaderInfo: headerInfo,
            authenticationType: AuthenticationType.System,
            keepSession: true,
            tenant_id: null);

        userSession.SetPropertyValue(x => x.Id, GuidHelper.NewGuidZeroPrefixed());
        return userSession;
    }

    protected override IUser GetUser()
    {
        return DefaultAdminUserConstants.User;
    }
}
