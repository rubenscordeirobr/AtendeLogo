using System.Net;
using System.Reflection;
using AtendeLogo.Presentation.Common.Enums;
using AtendeLogo.Presentation.Common.Exceptions;
using AtendeLogo.Shared.Contracts;

namespace AtendeLogo.Presentation.Common;

public class HttpMethodDescriptor
{
    public MethodInfo Method { get; }
    public HttpMethodAttribute Attribute { get; }
    public ParameterInfo[] Parameters { get; }
    public ParameterInfo[] RouteParameters { get; }
    public bool HasCancellationToken { get; }
    public string RouteTemplate { get; }
    public string QueryTemplate { get; }
    public bool IsBodyParameter { get; }
    public Type ResponseType { get; }

    public IReadOnlyList<ParameterInfo> QueryParameters
        => ParameterToQueryKeyMap.Keys.ToList();

    public IReadOnlyDictionary<ParameterInfo, string> ParameterToQueryKeyMap { get; }

    public HttpStatusCode SuccessStatusCode
        => Attribute.SuccessStatusCode;

    public HttpVerb HttpVerb
        => this.Attribute.HttpVerb;
     
    public Type? BodyType
        => IsBodyParameter
            ? Parameters.FirstOrDefault()?.ParameterType
            : null;

    public HttpMethodDescriptor(
        MethodInfo method)
    {
        var parameters = method.GetParameters();
        var lastParameter = parameters.LastOrDefault();

        Method = method;
        Attribute = method.GetCustomAttribute<HttpMethodAttribute>()
            ?? throw new HttpTemplateException("Method must have an HttpMethodAttribute.");

        HasCancellationToken = lastParameter is not null 
            && lastParameter.ParameterType == typeof(CancellationToken);

        Parameters = HasCancellationToken
            ? parameters[..^1]
            : parameters;
         
        IsBodyParameter = IsBodyParameterPresent();
        RouteTemplate = Attribute.RouteTemplate;
        RouteParameters = GetRouteParameters();
        QueryTemplate = AdjustQueryTemplate(Attribute.QueryTemplate);
        ParameterToQueryKeyMap = MapParametersToQueryKeys();
        ResponseType = ResolveResponseType(Method.ReturnType);
        ParameterValidator.Validate(this);
    }

    private bool IsBodyParameterPresent()
    {
        return Parameters.Length == 1 &&
               Parameters[0].
               ParameterType.ImplementsGenericInterfaceDefinition(typeof(IRequest<>));
    }

    private string AdjustQueryTemplate(string queryTemplate)
    {
        var needsCreateQueryTemplate = string.IsNullOrEmpty(queryTemplate) &&
            Attribute.HttpVerb == HttpVerb.Get &&
            Parameters.Length > 0 &&
            !IsBodyParameter;

        if (needsCreateQueryTemplate)
            return CreateQueryTemplate();

        return queryTemplate;
    }

    private string CreateQueryTemplate()
    {
        var queryParameters = Parameters.Except(RouteParameters)
            .Select(x => $"{x.Name}={{{x.Name}}}");

        return string.Join("&", queryParameters);
    }

    private ParameterInfo[] GetRouteParameters()
    {
        if (Parameters.Length == 0)
            return [];

        var routeParameters = new List<ParameterInfo>();
        var routeParts = RouteTemplate.Split('/', StringSplitOptions.RemoveEmptyEntries);
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

    protected IReadOnlyDictionary<ParameterInfo, string> MapParametersToQueryKeys()
    {
        if (Parameters.Length == 0)
        {
            return new Dictionary<ParameterInfo, string>();
        }

        var mappings = new Dictionary<ParameterInfo, string>();
        var pairs = QueryTemplate.Split('&', StringSplitOptions.RemoveEmptyEntries);
      
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

    private Type ResolveResponseType(Type currentType)
    {
        if(currentType.IsGenericType)
        {
            if (currentType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                return ResolveResponseType(currentType.GenericTypeArguments[0]);
            }

            if (currentType.GetGenericTypeDefinition() == typeof(Result<>))
            {
                return ResolveResponseType(currentType.GenericTypeArguments[0]);
            }
        }
        return currentType;
    }
}
