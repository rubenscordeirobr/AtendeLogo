using AtendeLogo.TenantPortal.BlazorServer.Extensions;
using Microsoft.AspNetCore.Authentication;

namespace AtendeLogo.TenantPortal.BlazorServer.Services;

internal sealed class HttpContextSignInHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<HttpContextSignInHandler> _logger;

    public HttpContextSignInHandler(
        IHttpContextAccessor httpContextAccessor, 
        ILogger<HttpContextSignInHandler> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task HandleUserAuthenticationAsync(UserSessionState? userSessionState)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is null)
        {
            _logger.LogError("HttpContext is null. Cannot handle authentication.");
            return;
        }
         
        var userSessionClaims = userSessionState?.UserSessionClaims;
        var authorizationToken = userSessionState?.AuthorizationToken;

        if (userSessionClaims is null ||
            string.IsNullOrWhiteSpace(authorizationToken) ||
            userSessionClaims.IsAnonymous())
        {
            await SendSingOutRequestAsync(httpContext);
            return;
        } 
        if (httpContext.Response.Headers.IsReadOnly)
        {
            await SendSignInRequestAsync(httpContext, authorizationToken);
            return;
        }

        var authResult = await httpContext.AuthenticateAsync(TenantUserAuthenticationConfig.AuthenticationScheme);
        if (authResult.Succeeded)
        {
            return;
        }

        var principal = userSessionClaims!.ToClaimsPrincipal();
        await httpContext.SignInAsync(TenantUserAuthenticationConfig.AuthenticationScheme, principal);
    }

    private async Task SendSignInRequestAsync(
        HttpContext httpContext,
        string authorizationToken)
    {
        var baseUrl = httpContext.Request.GetBaseUrl();
        var loginEndpoint = $"{baseUrl}{RouteBlazorServerConstants.BlazorServerLoginRoute}";
        var authorizationTokenHeader = JwtUtils.FormatAsAuthorizationHeader(authorizationToken);

        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, loginEndpoint);
        requestMessage.Headers.Add("Authorization", authorizationTokenHeader);
        requestMessage.Headers.Add("Accept", "application/json");

        await SendRequestMessageAsync(httpContext, requestMessage);
    }

    private async Task SendSingOutRequestAsync(HttpContext httpContext)
    {
        var baseUrl = httpContext.Request.GetBaseUrl();
        var logoutEndpoint = $"{baseUrl}{RouteBlazorServerConstants.BlazorServerLoginRoute}";
        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, logoutEndpoint);
        requestMessage.Headers.Add("Accept", "application/json");
        await SendRequestMessageAsync(httpContext, requestMessage);
    }

    private async Task SendRequestMessageAsync(
        HttpContext httpContext,
        HttpRequestMessage requestMessage)
    {
        using var httpClient = new HttpClient();
        var messageResult = await httpClient.SendAsync(requestMessage);
        if (!messageResult.IsSuccessStatusCode)
        {
            _logger.LogError("Error initiating RequestUri {RequestUri}. {StatusCode} {ReasonPhrase}",
                requestMessage.RequestUri,
                messageResult.StatusCode,
                messageResult.ReasonPhrase);
        }
    }
}

