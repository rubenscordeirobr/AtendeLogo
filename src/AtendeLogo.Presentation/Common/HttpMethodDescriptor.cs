using System.Net;
using System.Reflection;
using AtendeLogo.Presentation.Common.Enums;
using AtendeLogo.Presentation.Common.Exceptions;

namespace AtendeLogo.Presentation.Common;

public class HttpMethodDescriptor
{
    public MethodInfo Method { get; }
    public HttpMethodAttribute Attribute { get; }
    public ParameterInfo[] Parameters { get; }
    public ParameterInfo[] RouteParameters { get; }
    public IReadOnlyDictionary<ParameterInfo, string> ParameterToQueryKeyMap { get; }
    public bool HasCancellationToken { get; }

    public string RouteTemplate
        => this.Attribute.RouteTemplate;

    public string QueryTemplate
        => this.Attribute.QueryTemplate;

    public HttpStatusCode SuccessStatusCode
        => Attribute.SuccessStatusCode;

    public HttpVerb HttpVerb
        => this.Attribute.HttpVerb;

    public HttpMethodDescriptor(
        MethodInfo method)
    {
        var paramerters = method.GetParameters();

        Method = method;
        Attribute = method.GetCustomAttribute<HttpMethodAttribute>()
            ?? throw new HttpTemplateException("Method must have an HttpMethodAttribute.");

        HasCancellationToken = paramerters.LastOrDefault()?
            .ParameterType == typeof(CancellationToken);

        Parameters = HasCancellationToken
            ? paramerters[..^1]
            : paramerters;

        RouteParameters = GetRouteParameters(RouteTemplate);
        ParameterToQueryKeyMap = MapKeyKeyToParameters(Attribute.QueryTemplate);

        ParameterValidator.Validate(this);
    }

    private ParameterInfo[] GetRouteParameters(string routeTemplate)
    {
        if (Parameters.Length == 0)
        {
            return [];
        }

        var routeParameters = new List<ParameterInfo>();
        var routeParts = routeTemplate.Split('/', StringSplitOptions.RemoveEmptyEntries);
        foreach (var routePart in routeParts)
        {
            if (routePart.Contains("{") && routePart.Contains("}"))
            {
                var parameterName = ExtractParameterName(routePart);
                var parameter = Parameters.FirstOrDefault(x => x.Name == parameterName);
                if (parameter is null)
                {
                    throw new RouteTemplateException(
                        $"Error binding route template '{RouteTemplate}' in method '{Method.Name}' of type '{Method.DeclaringType?.Name}': " +
                        $"the route segment '{{{parameterName}}}' does not match any parameter in the method signature. " +
                        "Please verify that the route template references an existing parameter.");
                }
                routeParameters.Add(parameter);
            }
        }
        return [.. routeParameters];
    }

    private string? ExtractParameterName(string routePart)
    {
        var start = routePart.IndexOf("{");
        var end = routePart.IndexOf("}");
        if (end > start)
        {
            return routePart.Substring(start + 1, end - start - 1);
        }

        throw new RouteTemplateException(
            $"Error binding route template '{RouteTemplate}' in method '{Method.Name}' of type '{Method.DeclaringType?.Name}': " +
            $"the route segment '{routePart}' is invalid. It must be in the format '{{parameterName}}'. " +
            "Please review the route template syntax.");
    }

    protected IReadOnlyDictionary<ParameterInfo, string> MapKeyKeyToParameters(string pathOrQuery)
    {
        if (Parameters.Length == 0)
        {
            return new Dictionary<ParameterInfo, string>();
        }

        var mappings = new Dictionary<ParameterInfo, string>();
        var pairs = pathOrQuery.Split('&', StringSplitOptions.RemoveEmptyEntries);
        foreach (var pair in pairs)
        {
            var keyValue = pair.Split('=', StringSplitOptions.RemoveEmptyEntries);
            if (keyValue.Length != 2)
            {
                throw new RouteTemplateException(
                    $"Error binding query template '{QueryTemplate}' in method '{Method.Name}' of type '{Method.DeclaringType?.Name}': " +
                    $"the key-value pair '{pair}' is invalid. It must be in the format 'key={{parameterName}}'. " +
                    "Please review the query template syntax.");
            }

            var queryKey = keyValue[0];
            var value = keyValue[1];
            if (value.StartsWith("{") && value.EndsWith("}"))
            {
                var parameterName = value.Substring(1, value.Length - 2);
                var parameter = Parameters.FirstOrDefault(x => x.Name == parameterName);
                if (parameter is null)
                {
                    throw new QueryTemplateException(
                        $"Error binding query template '{QueryTemplate}' in method '{Method.Name}' of type '{Method.DeclaringType?.Name}': " +
                        $"the parameter '{parameterName}' specified in the template is not present in the method signature. " +
                        "Ensure that all parameters referenced in the query template are defined in the method.");
                }
                mappings[parameter] = queryKey;
            }
        }
        return mappings;
    }
}
