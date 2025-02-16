using AtendeLogo.Common.Infos;
using Microsoft.AspNetCore.Http;

namespace AtendeLogo.Infrastructure.Extensions;

public static class HttpContextExtensions
{
    public static RequestHeaderInfo GetRequestHeaderInfo(
        this HttpContext context)
    {
        if (context == null)
        {
            return RequestHeaderInfo.Unknown;
        }

        var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        var userAgent = context.Request.Headers["User-Agent"].ToString() ?? "Unknown";
        var applicationName = context.Request.Headers["Application-Name"].ToString() ?? "Unknown";

        return new RequestHeaderInfo
        (
            IpAddress : ipAddress,
            UserAgent : userAgent,
            ApplicationName :applicationName
        );
    }
}
