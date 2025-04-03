using System.Reflection;
using AtendeLogo.Common.Converters;
using AtendeLogo.Common.Extensions;
using AtendeLogo.Common.Utils;
using AtendeLogo.Shared.Contracts;

namespace AtendeLogo.ClientGateway.Common.Helpers;

internal static class HttpClientHelper
{
    public static string CreateQueryString(IQueryRequest query)
    {
        var properties = query.GetType().GetProperties();
        var queryData = new List<KeyValuePair<string, string>>();
        foreach (var prop in properties)
        {
            var value = OperatorParameterConverter.ToString(prop.GetValue(query), prop.PropertyType);
            var key = OperationParameterUtils.NormalizeKey(prop.Name);
            queryData.Add(new KeyValuePair<string, string>(key, Uri.EscapeDataString(value ?? "")));
        }
        return string.Join("&", queryData.Select(kv => $"{kv.Key}={kv.Value}"));
    }

    public static IReadOnlyList<KeyValuePair<string, string>> CreateFormKeyValuePairs(
        string[] parameterNames,
        object[] parameterValues)
    {
        var keyValuePairs = new List<KeyValuePair<string, string>>();
        for (var i = 0; i < parameterNames.Length; i++)
        {
            var value = OperatorParameterConverter.ToString(parameterValues[i], parameterValues[i].GetType());
            if (!string.IsNullOrEmpty(value))
            {
                var key = OperationParameterUtils.NormalizeKey(parameterNames[i]);
                keyValuePairs.Add(new KeyValuePair<string, string>(key, value));
            }
        }
        return keyValuePairs;
    }

    internal static bool MethodAllowBody(HttpMethod method)
    {
        return method == HttpMethod.Post ||
            method == HttpMethod.Put ||
            method == HttpMethod.Patch ||
            method == HttpMethod.Delete;
    }

    internal static void ThrowIfMethodNotAllowBody(
        HttpMethod method,
        Uri requestUri)
    {
        if (!HttpClientHelper.MethodAllowBody(method))
        {
            throw new InvalidOperationException(
                $"Method '{method}' does not allow body. URI: {requestUri}");
        }
    }
}

public static class RouteBinder
{
    internal static string GetRoute<T>()
    {
        var type = typeof(T);
        var attribute = type.GetCustomAttribute<RouteAttribute>();
        if (attribute is null)
        {
            throw new InvalidOperationException($"Type '{type.Name}' does not have a RouteAttribute.");
        }
        return attribute.Route;
    }
 
    public static string? BindRoute(
        object obj,
        string? routeTemplate)
    {
        if (string.IsNullOrEmpty(routeTemplate))
        {
            return routeTemplate;
        }

        var binder = new RouteTemplateBinder(routeTemplate);
        return binder.Bind(obj);
    }

    internal static string? BindRoute(
       string[] parameterNames,
       object[] parameterValues,
       string? routeTemplate)
    {
        if (string.IsNullOrEmpty(routeTemplate))
        {
            return routeTemplate;
        }

        var binder = new RouteTemplateBinder(routeTemplate);
        return binder.Bind(parameterNames, parameterValues);
    }

    private sealed class RouteTemplateBinder
    {
        private readonly int _start;
        private readonly int _close;

        public string Route { get; }
        public string? ParameterName { get; }

        internal RouteTemplateBinder(string route)
        {
            Route = route;
            _start = route.IndexOf('{', StringComparison.Ordinal);
            _close = route.IndexOf('}', StringComparison.Ordinal);

            if (_start >= 0 && _close > _start)
            {
                ParameterName = route.Substring(_start + 1, _close - _start - 1);
            }
        }

        public string Bind(object obj)
        {
            if (ParameterName is null)
            {
                return Route;
            }

            Guard.NotNull(obj);
            
            var property = obj.GetType().GetPropertyByName(ParameterName);
            if (property is null)
            {
                throw new InvalidOperationException(
                    $"Property '{ParameterName}' for route template {Route} was not found in type '{obj.GetType().Name}'.");
            }
            try
            {
                var value = property.GetValue(obj);
                return BindValue(value);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Error binding property '{ParameterName}' for route template {Route} in type '{obj.GetType().Name}'.", ex);
            }
        }

        public string Bind(
            string[] parameterNames,
            object[] parameterValues)
        {
            if (ParameterName is null)
            {
                return Route;
            }
            var index = Array.IndexOf(parameterNames, ParameterName);
            if (index == -1)
            {
                throw new InvalidOperationException(
                    $"Parameter '{ParameterName}' for route template {Route} was not found in the parameter names.");
            }
            return BindValue(parameterValues[index]);
        }

        private string BindValue(object? value)
        {
            return $"{Route.Substring(0, _start)}{value}{Route.Substring(_close + 1)}";
        }
    }
}

