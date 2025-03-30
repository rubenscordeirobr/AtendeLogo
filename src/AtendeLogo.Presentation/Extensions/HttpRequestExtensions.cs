using Microsoft.AspNetCore.Http;

namespace AtendeLogo.Presentation.Extensions;

internal static class HttpRequestExtensions
{
    internal static List<string> GetOperationKeys(this HttpRequest request)
    {
        var query = request.Query;
        if (query?.Count > 0)
        {
            return [.. query.Keys];
        }

        if (request.HasFormContentType)
        {
            return [.. request.Form.Keys];
        }
        return [];
    }

    internal static string GetPathAndQueryString(
        this HttpRequest httpRequest)
    {
        var path = httpRequest.Path.Value;
        var queryString = httpRequest.QueryString.Value;
        return $"{path}{queryString}";
    }
}


