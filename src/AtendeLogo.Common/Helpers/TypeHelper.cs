﻿namespace AtendeLogo.Common.Helpers;

public static class TypeHelper
{
    public static IEnumerable<(Type, Type)> FindTypesImplementingInterface(
        Type[] types,
        Type interfaceDefinitionType,
        bool isConcrete = true)
    {
        Guard.NotNull(types);
        Guard.NotNull(interfaceDefinitionType);

        var typeMappings = new List<(Type, Type)>();

        var queryType = types
           .Where(type => type.ImplementsGenericInterfaceDefinition(interfaceDefinitionType));

        if (isConcrete)
        {
            queryType = queryType.Where(type => type.IsConcrete());
        }

        foreach (var type in queryType.ToList())
        {
            var interfaces = type.GetInterfaces();

            var implementedInterface = interfaces
               .FirstOrDefault(type => type.IsGenericType &&
                    type.GetGenericTypeDefinition() == interfaceDefinitionType);

            if (implementedInterface == null)
            {
                var message = $"Interface {type.Name} does not implement {interfaceDefinitionType.Name}";
                throw new InvalidOperationException(message);
            }
            typeMappings.Add((type, implementedInterface));
        }
        return typeMappings;
    }
}
