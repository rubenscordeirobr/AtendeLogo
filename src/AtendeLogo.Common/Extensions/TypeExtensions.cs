using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace AtendeLogo.Common.Extensions;

public static class TypeExtensions
{
    public static bool IsSubclassOf<T>(this Type type)
    {
        return type.IsSubclassOf(typeof(T));
    }

    public static bool IsSubclassOfOrEquals<T>(this Type type)
    {
        return type.IsSubclassOfOrEquals(typeof(T));
    }
    public static bool IsSubclassOfOrEquals(this Type type, Type otherType)
    {
        return type.IsSubclassOf(otherType) || type == otherType;
    }

    public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
    {
        if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
        {
            return true;
        }
        if (givenType.GetInterfaces().Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == genericType))
        {
            return true;
        }
        if (givenType.BaseType is null)
        {
            return false;
        }
        return givenType.BaseType.IsAssignableToGenericType(genericType);
    }

    public static IEnumerable<PropertyInfo> GetDeclaredProperties(
        this Type type,
        bool isIgnoreNotMappedAtribute = true)
    {
        var properties = type.GetProperties()
            .Where(x => x.DeclaringType == type);

        return (isIgnoreNotMappedAtribute)
            ? properties.Where(property => property.GetCustomAttribute<NotMappedAttribute>() == null)
            : properties;
    }
    
    public static IEnumerable<PropertyInfo> GetDeclaredPropertiesOfType<T>(
        this Type type, bool isIgnoreNotMappedAtribute = true)
    {
        return GetDeclaredPropertiesOfType(type, typeof(T), isIgnoreNotMappedAtribute);
    }

    public static IEnumerable<PropertyInfo> GetDeclaredPropertiesOfType(
        this Type type,
        Type propertyType,
        bool isIgnoreNotMappedAtribute = true)
    {
        var properties =  type.GetProperties()
            .Where(x => x.DeclaringType == type)
            .Where(x => x.PropertyType.IsSubclassOfOrEquals(propertyType));

        return (isIgnoreNotMappedAtribute)
            ? properties.Where(property => property.GetCustomAttribute<NotMappedAttribute>() == null)
            : properties;
    }
}
