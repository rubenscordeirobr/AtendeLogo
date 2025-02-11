using System.Reflection;

namespace AtendeLogo.Common.Extensions;

public static class PropertyInfoExtesions
{
    public static string GetPropertyPath(this PropertyInfo propertyInfo)
    {
        return $"{propertyInfo.DeclaringType?.GetQualifiedTypeName()}::{propertyInfo.Name}";
    }
}
