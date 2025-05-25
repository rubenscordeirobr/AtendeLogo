using System.IdentityModel.Tokens.Jwt;
using AtendeLogo.Shared.Configuration;
using AtendeLogo.Shared.Models.Security;
using Microsoft.AspNetCore.Authentication;

namespace AtendeLogo.TenantPortal.BlazorServer.Services;

internal sealed class HttpContextSignInService
{
    private readonly JwtSecurityTokenHandler _tokenHandler = new();
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<HttpContextSignInService> _logger;

    public HttpContextSignInService(
        IHttpContextAccessor httpContextAccessor,
        ILogger<HttpContextSignInService> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task LoginAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is null)
        {
            _logger.LogError("HttpContext is null. Cannot handle authentication.");
            return;
        }

        var headerInfo = httpContext.GetRequestHeaderInfo();
        if (string.IsNullOrWhiteSpace(headerInfo.AuthorizationToken))
        {
            return;
        }

        var userSessionClaims = ReadToken(headerInfo.AuthorizationToken);
        if (userSessionClaims is null)
        {
            return;
        }

        var principal = userSessionClaims.ToClaimsPrincipal();
        var expiration = UserSessionConfig.GetSessionExpirationDateUtc(userSessionClaims.IsPersistent);
        var properties = new AuthenticationProperties
        {
            IsPersistent = userSessionClaims.IsPersistent,
            ExpiresUtc = expiration
        };

        try
        {
            await httpContext.SignInAsync(
                scheme: TenantUserAuthenticationConfig.AuthenticationScheme,
                principal,
                properties);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error signing in user. {ErrorMessage}", ex.GetNestedMessage());
            return;
        }
    }

    internal async Task LogoutAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is null)
        {
            _logger.LogError("HttpContext is null. Cannot handle authentication.");
            return;
        }

        try
        {
            await httpContext.SignOutAsync(TenantUserAuthenticationConfig.AuthenticationScheme);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error signing out user. {ErrorMessage}", ex.GetNestedMessage());
        }
    }

    private UserSessionClaims? ReadToken(string token)
    {
        var jwtToken = _tokenHandler.ReadJwtToken(token);
        var result = UserSessionClaimsFactory.Create(jwtToken.Claims, jwtToken.ValidTo);
        if (result.IsSuccess)
        {
            return result.Value;
        }

        _logger.LogError("Error reading user session token. {Token}. Code: {Code}, Message: {Message} ",
            token,
            result.Error.Code,
            result.Error.Message);

        return null;
    }
}
