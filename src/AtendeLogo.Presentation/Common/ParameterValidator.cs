using System.Reflection;
using AtendeLogo.Presentation.Common.Exceptions;
using AtendeLogo.Shared.Contracts;

namespace AtendeLogo.Presentation.Common;

internal class ParameterValidator
{
    private static readonly Type[] SupportedTypes = {
            typeof(bool),
            typeof(byte),
            typeof(sbyte),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(float),
            typeof(double),
            typeof(decimal),
            typeof(char),
            typeof(string),
            typeof(Guid),
            typeof(DateTime),
            typeof(TimeSpan)
        };

    internal static void Validate(HttpMethodDescriptor descriptor)
    {
        var parameters = descriptor.Parameters;
        var routeParameters = descriptor.RouteParameters;
        var queryParameters = descriptor.ParameterToQueryKeyMap.Keys;
        var method = descriptor.Method;

        if (parameters.Length > 0)
        {
            // if the method has only one parameter and it implements IRequest<>, 
            // it is assumed to be bound from the request body.
            if (parameters.Length == 1 &&
                parameters[0].
                ParameterType.ImplementsGenericInterfaceDefinition(typeof(IRequest<>)))
            {
                return;
            }
        }

        var allParameters = routeParameters
            .Concat(queryParameters)
            .ToList();

        var missingParameters = parameters.Where(p => !p.HasDefaultValue && !allParameters.Contains(p)).ToList();

        if (missingParameters.Any())
        {
            throw new HttpTemplateException(
                $"Error binding HTTP templates in method '{method.Name}' of type '{method.DeclaringType?.Name}': " +
                $"the following parameters are not mapped in the route or query template: {string.Join(", ", missingParameters)}. " +
                "Please ensure that every non-CancellationToken parameter in the method signature is referenced in the route and/or query template.");
        }

        var parameterDuplicate = routeParameters
            .Intersect(queryParameters)
            .ToList();

        if (parameterDuplicate.Any())
        {
            throw new HttpTemplateException(
                $"Error binding HTTP templates in method '{method.Name}' of type '{method.DeclaringType?.Name}': " +
                $"the following parameters are mapped in both the route and query template: {string.Join(", ", parameterDuplicate)}. " +
                "Please ensure that each parameter is mapped to only one template.");
        }

        allParameters.ForEach(parameter => ValidateParameter(method, parameter));
    }

    private static void ValidateParameter(MethodInfo method, ParameterInfo paramerter)
    {
        ValidateParameterType(method, paramerter.ParameterType);
    }

    public static void ValidateParameterType(MethodInfo method, Type parameterType)
    {
        // Unwrap nullable types
        parameterType = parameterType.GetUnderlyingType();

        if (parameterType == typeof(CancellationToken))
        {
            return;
        }

        if (parameterType.IsPrimitive ||
            parameterType.IsEnum ||
            Array.Exists(SupportedTypes, t => t == parameterType))
        {
            return;
        }

        throw new HttpTemplateException(
            $"Error binding HTTP templates in method '{method.Name}' of type '{method.DeclaringType?.Name}': " +
            $"the parameter type '{parameterType.Name}' is not supported. " +
            "Please ensure that all parameters are one of the following types: " +
            $"{string.Join(", ", SupportedTypes.Select(t => t.Name))} " +
            "or an enum type.");
    }
}
