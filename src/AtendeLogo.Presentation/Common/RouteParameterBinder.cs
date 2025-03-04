﻿using System.Reflection;
using Microsoft.AspNetCore.Http;

namespace AtendeLogo.Presentation.Common;

public static class RouteParameterBinder
{
    public static Result<object> BindParameter(
        HttpMethodDescriptor descriptor,
        HttpContext context,
        ParameterInfo parameter)
    {
        var parameterType = parameter.ParameterType;
        var parameterName = parameter.Name;

        if (string.IsNullOrWhiteSpace(parameterName))
        {
            return Result.Failure<object>(new BadRequestError(
                "ParameterNameMissing",
                $"Error in method '{descriptor.Method.Name}' of '{descriptor.Method.DeclaringType?.Name}': " +
                "A route parameter is missing a name in the descriptor with route template " +
                $"'{descriptor.RouteTemplate}'."));
        }

        if (context.Request.RouteValues.TryGetValue(parameterName, out var routeValue))
        {
            try
            {
                var convertedValue = ParameterConverter.Parse(routeValue?.ToString(), parameter.ParameterType);
                return Result.Success(convertedValue!);
            }
            catch (Exception ex)
            {
                return Result.Failure<object>(
                    new BadRequestError(
                        "ParameterConversionFailed",
                        $"Failed to convert route value '{routeValue}' in method '{descriptor.Method.Name}' " +
                        $"of '{descriptor.Method.DeclaringType?.Name}'. Expected parameter '{parameterName}' of type " +
                        $"'{parameter.ParameterType}', but received an incompatible value from route template '{descriptor.RouteTemplate}'. " +
                        $"Details: {ex.GetNestedMessage()}"));
            }
        }

        if (parameter.HasDefaultValue)
        {
            return Result.Success(parameter.DefaultValue!);
        }

        return Result.Failure<object>(
            new BadRequestError(
                "ParameterNotFound",
                $"Error in method '{descriptor.Method.Name}' of '{descriptor.Method.DeclaringType?.Name}': " +
                $"Missing required route parameter '{parameterName}' in route template '{descriptor.RouteTemplate}'."));
    }
}
