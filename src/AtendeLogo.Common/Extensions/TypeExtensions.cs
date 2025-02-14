﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace AtendeLogo.Common.Extensions;

public static class TypeExtensions
{
    public static bool IsConcrete(this Type type)
    {
        return !type.IsAbstract && !type.IsInterface;
    }

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
      
    public static bool ImplementsGenericInterfaceDefinition(
        this Type type,
        Type definitionType)
    {
        if (!definitionType.IsGenericType)
        {
            return false;
        }

        if (!definitionType.IsGenericTypeDefinition)
        {
            definitionType = definitionType.GetGenericTypeDefinition();
        }

        if (type.IsGenericType &&
            type.GetGenericTypeDefinition() == definitionType)
        {
            return true;
        }

        return type.GetInterfaces()
            .Any(interfaceType => interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == definitionType);
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
        var properties = type.GetProperties()
            .Where(x => x.DeclaringType == type)
            .Where(x => x.PropertyType.IsSubclassOfOrEquals(propertyType));

        return (isIgnoreNotMappedAtribute)
            ? properties.Where(property => property.GetCustomAttribute<NotMappedAttribute>() == null)
            : properties;
    }

    public static IEnumerable<Type> GetAssignableTypes(this Type eventType)
    {
        var assignableTypes = new List<Type> { eventType };
        assignableTypes.AddRange(eventType.GetInterfaces());
        if (eventType.BaseType != null && eventType.BaseType != typeof(object))
        {
            assignableTypes.Add(eventType.BaseType);
            assignableTypes.AddRange(eventType.BaseType.GetAssignableTypes());
        }
        return assignableTypes;
    }

    public static string GetQualifiedName(this Type type)
    {
        return GetQualifiedTypeNameInternal(type);

        static string GetQualifiedTypeNameInternal(Type type)
        {
            if (type.IsGenericType)
            {
                var genericArguments = type.GetGenericArguments()
                    .Select(GetQualifiedName);

                return $"{type.Name.Split('`')[0]}<{string.Join(", ", genericArguments)}>";
            }
            if (type.FullName is not null)
                return type.FullName;

            if(type.Namespace is not null)
            {
                return string.IsNullOrWhiteSpace(type.Namespace)
                ? type.Name
                : $"{type.Namespace}.{type.Name}";
            }
            return type.Name;
           
        }
    }

    public static IDictionary<string, PropertyInfo> GetPropertiesFromInterface<TInterface>(
     this Type type)

    {
        return GetPropertiesFromInterface(type, typeof(TInterface));
    }

    public static IDictionary<string, PropertyInfo> GetPropertiesFromInterface(
     this Type type,
     Type interfaceType)

    {
        if (!type.IsAssignableTo(interfaceType))
        {
            var message = $"The type {type.Name} does not implement the interface {interfaceType.Name}";
            throw new InvalidOperationException(message);
        }

        var properties = type.GetProperties();
        var interfaceProperties = interfaceType.GetProperties();
        var result = new Dictionary<string, PropertyInfo>();

        var interfacePropertyNames = new HashSet<string>(
           interfaceType.GetProperties().Select(p => p.Name));

        static int GetInheritanceDistance(Type derivedType, Type? declaringType)
        {
            int distance = 0;
            var currentType = derivedType;
            while (currentType != null && currentType != declaringType)
            {
                distance++;
                currentType = currentType.BaseType;
            }
            return currentType == declaringType ? distance : int.MaxValue;
        }

        var interfacePropertyMappings = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => interfacePropertyNames.Contains(p.Name))
            .GroupBy(p => p.Name)
            .ToDictionary(
                group => group.Key,
                group => group.OrderBy(p => GetInheritanceDistance(type, p.DeclaringType)).First()
            );

        if (interfaceProperties.Any(p => !interfacePropertyMappings.ContainsKey(p.Name)))
        {
            var message = $"The properties {string.Join(", ", interfaceProperties.Select(p => p.Name))} from interface {interfaceType.Name} are not implemented in {type.Name}";
            throw new InvalidOperationException(message);
        }

        return interfacePropertyMappings;
    }
}
