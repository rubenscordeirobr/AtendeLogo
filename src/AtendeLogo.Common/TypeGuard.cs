using System.Runtime.CompilerServices;
using AtendeLogo.Common.Extensions;

namespace AtendeLogo.Common;

public class TypeGuard
{
    public static void MustBeNotGeneric(Type type,
          [CallerArgumentExpression("type")] string? paramName = "")
    {
        if (type.IsGenericType)
            throw new InvalidOperationException($"{paramName}: Type{type.Name} is a generic type");
    }

    public static void MustBeConcrete(Type type,
        [CallerArgumentExpression("type")] string? paramName = "")
    {
        if (!type.IsConcrete())
            throw new InvalidOperationException($"{paramName}: Type{type.Name} must be a concrete type");
    }

    public static void TypeMustBeAssinableFrom(Type type, Type other)
    {
        if (!other.IsAssignableFrom(type))
            throw new InvalidOperationException($"{type.Name} must be a subclass of {other.Name}");

    }

    public static void TypeMustubclassOfOrEquals(Type type, Type other)
    {
        if (!type.IsSubclassOfOrEquals(other))
            throw new InvalidOperationException($"{type.Name} must be a subclass of {other.Name}");

    }

    public static void TypeMustBeInterface(Type type, Type other)
    {
        if (!other.IsInterface)
            throw new InvalidOperationException($"{other.Name} must be an interface");
    }

    public static void TypeMustImplementInterface(Type type, Type interfaceType)
    {
        if (type.ImplementsGenericInterfaceDefinition(interfaceType))
        {
            return;
        }

        if (!interfaceType.IsAssignableFrom(type))
        {
            throw new InvalidOperationException($"{type.Name} must implement {interfaceType.Name}");
        }
    }
}
