using System.Reflection;
using Microsoft.AspNetCore.Http;

namespace AtendeLogo.Presentation.Common;

public static class QueryParameterBinder
{
    public static Result<object> BindParameter(
        HttpMethodDescriptor descriptor,
        HttpContext httpContext,
        ParameterInfo parameter,
        string queryKey)
    {
        var parameterName = parameter.Name;

        if (string.IsNullOrWhiteSpace(queryKey))
        {
            return Result.Failure<object>(new BadRequestError(
                "QueryKeyMissing",
                $"Error in method '{descriptor.Method.Name}' of '{descriptor.Method.DeclaringType?.Name}': " +
                "A query parameter is missing a name in the descriptor with query template " +
                $"'{descriptor.QueryTemplate}'."));
        }

        if (httpContext.Request.Query.TryGetValue(queryKey, out var queryValue) && queryValue.Count > 0)
        {
            var queryValueString = queryValue[0];

            try
            {
                var convertedValue = ParameterConverter.Parse(queryValueString, parameter.ParameterType);
                return Result.Success(convertedValue!);
            }
            catch (Exception ex)
            {
                return Result.Failure<object>(new BadRequestError(
                    "ParameterConversionFailed",
                    $"Error in method '{descriptor.Method.Name}' of '{descriptor.Method.DeclaringType?.Name}': " +
                    $"Failed to convert query parameter '{parameterName}' (from query template '{descriptor.QueryTemplate}') " +
                    $"to type '{parameter.ParameterType.Name}'. Details: {ex.GetNestedMessage()}"));
            }
        }

        if (parameter.HasDefaultValue)
        {
            return Result.Success(parameter.DefaultValue!);
        }

        return Result.Failure<object>(new BadRequestError(
            "ParameterNotFound",
            $"Error in method '{descriptor.Method.Name}' of '{descriptor.Method.DeclaringType?.Name}': " +
            $"Missing required query parameter '{parameterName}' in query template '{descriptor.QueryTemplate}'."));
    }
}
