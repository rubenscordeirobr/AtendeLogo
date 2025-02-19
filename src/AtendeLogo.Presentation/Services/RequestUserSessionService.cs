using AtendeLogo.Application.Factories;
using AtendeLogo.Shared.Interfaces.Identities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AtendeLogo.Presentation.Services;

public class RequestUserSessionService : IRequestUserSessionService
{
    private const int DefaultSessionExpirationDays = 7;
    private const string SessionTokenCookieName = "ClientSessionToken";

    private readonly HttpContext _httpContext;
    private readonly ILogger<RequestUserSessionService> _logger;

    public RequestUserSessionService(
        IHttpContextAccessor httpContextAccessor,
        ILogger<RequestUserSessionService> logger)
    {
        _httpContext = httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException("HttpContext is not available.");

        _logger = logger;
    }

    public IUserSession GetCurrentSession()
    {
        var currentSession = GetCurrentSessionInternal();
        if (currentSession != null)
        {
            return currentSession;
        }

        var headerInfo = _httpContext.GetRequestHeaderInfo();
        return UserSessionFactory.CreateAnonymousSession(headerInfo);
    }

    private IUserSession? GetCurrentSessionInternal()
    {
        try
        {
            if (_httpContext.Items.TryGetValue("UserSession", out var sessionObj) &&
                sessionObj is IUserSession userSession)
            {
                return userSession;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving session from HttpContext.Items.");
        }
        return null;
    }

    public RequestHeaderInfo GetRequestHeaderInfo()
    {
        return _httpContext.GetRequestHeaderInfo();
    }

    public void AddClientSessionCookie(string clientSessionToken)
    {
        try
        {
            var options = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(DefaultSessionExpirationDays),
                SameSite = SameSiteMode.Lax,
                Domain = _httpContext.Request.Host.Host
            };

            _httpContext.Response.Cookies.Append(
                SessionTokenCookieName,
                clientSessionToken,
                options);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding client session cookie.");
        }
    }

    public string? GetClientSessionToken()
    {
        if (_httpContext.Request.Cookies.TryGetValue(SessionTokenCookieName, out var sessionToken))
        {
            return sessionToken;
        }
        return null;
    }
}
