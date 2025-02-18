namespace AtendeLogo.Common.Helpers;

public static class TypeHelper
{
    public static IEnumerable<(Type, Type)> FindTypesImplementingInterface(
        Type[] types,
        Type interfaceDefinitionType,
        bool isConcrete = true)
    {
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
               .Where(type => type.IsGenericType)
               .Where(type => type.GetGenericTypeDefinition() == interfaceDefinitionType)
               .FirstOrDefault();

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
