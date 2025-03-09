using Microsoft.AspNetCore.Http;

namespace AtendeLogo.Presentation.Extensions;

public static class HttpRequestExtensions
{
    public static List<string> GetOperationKeys(this HttpRequest request)
    {
        var query = request.Query;
        if (query?.Any() == true)
        {
            return query.Keys.ToList();
        }

        if(request.HasFormContentType)
        {
            return request.Form.Keys.ToList();
        }
        return new List<string>();
    }
}


