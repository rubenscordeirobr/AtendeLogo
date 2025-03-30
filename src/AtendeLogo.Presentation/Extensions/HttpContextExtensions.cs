using AtendeLogo.Shared.Constants;
using Microsoft.AspNetCore.Http;

namespace AtendeLogo.Presentation.Extensions;

public static class HttpContextExtensions
{
    public static ClientRequestHeaderInfo GetRequestHeaderInfo(
        this HttpContext context)
    {
        Guard.NotNull(context);

        var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        var userAgent = context.Request.Headers[HttpHeaderConstants.UserAgent].ToString() ?? "Unknown";
        var applicationName = context.Request.Headers[HttpHeaderConstants.ApplicationName].ToString() ?? "Unknown";
        var authorizationHeader = context.Request.Headers.Authorization.ToString();
        var authorizationToken = JwtUtils.GetTokenFromAuthorizationHeader(authorizationHeader);
         
        return new ClientRequestHeaderInfo
        (
            IpAddress : ipAddress,
            UserAgent : userAgent,
            ApplicationName :applicationName,
            AuthorizationToken : authorizationToken
        );
    }
}

