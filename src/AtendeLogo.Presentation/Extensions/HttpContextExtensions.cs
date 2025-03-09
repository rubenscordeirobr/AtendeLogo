using Microsoft.AspNetCore.Http;

namespace AtendeLogo.Presentation.Extensions;

public static class HttpContextExtensions
{
    public static ClientRequestHeaderInfo GetRequestHeaderInfo(
        this HttpContext context)
    {
        Guard.NotNull(context);

        var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        var userAgent = context.Request.Headers["User-Agent"].ToString() ?? "Unknown";
        var applicationName = context.Request.Headers["Application-Name"].ToString() ?? "Unknown";

        return new ClientRequestHeaderInfo
        (
            IpAddress : ipAddress,
            UserAgent : userAgent,
            ApplicationName :applicationName
        );
    }
}
