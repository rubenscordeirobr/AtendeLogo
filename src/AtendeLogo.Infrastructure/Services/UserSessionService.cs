using AtendeLogo.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;

namespace AtendeLogo.Infrastructure.Services;

public class UserSessionService : IUserSessionService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserSessionService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public UserSession GetCurrentSession()
    {
        var currentSession = GetCurrentSessionInternal();
        if (currentSession != null)
        {
            return currentSession;
        }

        var context = _httpContextAccessor.HttpContext;
        var headerInfo = context.GetRequestHeaderInfo();
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

    private UserSession? GetCurrentSessionInternal()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context != null)
        {
            if (context != null &&
                context.Items.TryGetValue("UserSession", out var sessionObj) &&
                sessionObj is UserSession userSession)
            {
                return userSession;
            }
        }
        return null;
    }
}
