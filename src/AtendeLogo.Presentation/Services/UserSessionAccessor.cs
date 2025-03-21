using AtendeLogo.Presentation.Constants;
using AtendeLogo.Shared.Contracts;
using AtendeLogo.Shared.Factories;
using AtendeLogo.Shared.Interfaces.Identities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AtendeLogo.Presentation.Services;

public class UserSessionAccessor : IUserSessionAccessor
{
    private const int DefaultSessionExpirationDays = 7;
    private const string SessionTokenCookieName = "ClientSessionToken";

    private readonly HttpContext _httpContext;
    private readonly ILogger<UserSessionAccessor> _logger;

    public UserSessionAccessor(
        IHttpContextAccessor httpContextAccessor,
        ILogger<UserSessionAccessor> logger)
    {
        _httpContext = httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException("HttpContext is not available.");

        _logger = logger;
    }

    public IUserSession GetCurrentSession()
    {
        var currentSession = TryGetHttpContextItem<IUserSession>(HttpContextItensConstants.UserSession);
        if (currentSession != null)
        {
            return currentSession;
        }

        var headerInfo = _httpContext.GetRequestHeaderInfo();
        return AnonymousUserSessionFactory.CreateAnonymousSession(headerInfo);
    }
     
    public ClientRequestHeaderInfo GetClientRequestHeaderInfo()
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

    public void RemoveClientSessionCookie(string clientSessionToken)
    {
        try
        {
            _httpContext.Response.Cookies.Delete(SessionTokenCookieName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing client session cookie.");
        }
    }
     

    public IEndpointService? GetCurrentEndpointInstance()
    {
        return TryGetHttpContextItem<IEndpointService>(HttpContextItensConstants.EndpointInstance);
    }

    private TItem? TryGetHttpContextItem<TItem>(string key)
    {
        try
        {
            if (_httpContext.Items.TryGetValue(key,
                out var obj) &&
                obj is TItem ityem)
            {
                return ityem;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving item {typeof(TItem).FullName} from HttpContext.Items.");
        }
        return default;
    }
}
