using AtendeLogo.Presentation.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace AtendeLogo.Presentation.Common;

public static class HttpGetDescriptorSelector
{
    public static HttpMethodDescriptor Select(
        HttpContext httpContext,
        HttpMethodDescriptor[] descriptors)
    {
        Guard.NotEmpty(descriptors);

        if(descriptors.Length == 1)
        {
            return descriptors[0];
        }

        var requestQueryTemplate = CreateQueryTemplate(httpContext);
        var queryDescriptor = descriptors
            .Where(d => d.OperationTemplate.Equals(requestQueryTemplate, StringComparison.OrdinalIgnoreCase))
            .FirstOrDefault();

        if(queryDescriptor != null)
        {
            return queryDescriptor;
        }

        return SelectBetterQueryMatch(httpContext, descriptors, requestQueryTemplate);
    }

    public static string CreateQueryTemplate(HttpContext httpContext)
    {
        var query = httpContext.Request.Query;
        if (query == null || !query.Any())
        {
            return string.Empty;
        }

        var parameters = query.Keys.Select(key => $"{key}={{{key}}}");
        return string.Join("&", parameters);
    }

    private static HttpMethodDescriptor SelectBetterQueryMatch(
        HttpContext httpContext,
        HttpMethodDescriptor[] descriptors,
        string requestQueryTemplate)
    {
        var operationKeys = httpContext.Request.GetOperationKeys();

        HttpMethodDescriptor? bestMatch = null;
        int bestScore = -1;
        int bestKeyCountDiff = int.MaxValue;

        foreach (var descriptor in descriptors)
        {
            var descriptorOperationKeys = descriptor.OperationParameterToKeyMap.Values.ToArray();
          
            int commonKeysCount = operationKeys.Intersect(descriptorOperationKeys, StringComparer.OrdinalIgnoreCase).Count();
            int keyCountDiff = Math.Abs(operationKeys.Count - descriptorOperationKeys.Length);

            if (commonKeysCount > bestScore ||
                (commonKeysCount == bestScore && keyCountDiff < bestKeyCountDiff))
            {
                bestScore = commonKeysCount;
                bestKeyCountDiff = keyCountDiff;
                bestMatch = descriptor;
            }
        }

        if (bestMatch != null)
        {
            return bestMatch; 
        }

        throw new HttpTemplateException(
            $"Error selecting HTTP method descriptor for request '{httpContext.Request.Path}' with query '{requestQueryTemplate}': " +
            "no matching descriptor was found. Please ensure that the query template is correctly defined in the method signature."
         );
    }
}
