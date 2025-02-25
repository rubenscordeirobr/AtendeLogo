using System.Reflection;

namespace AtendeLogo.Common.Extensions;

public static class PropertyInfoExtensions
{
    public static string GetPropertyPath(this PropertyInfo propertyInfo)
    {
        return $"{propertyInfo.DeclaringType?.GetQualifiedName()}::{propertyInfo.Name}";
    }
}
