using System.Reflection;

namespace AtendeLogo.Common.Utils;

public static class ReflectionUtils
{

    public const BindingFlags AllInstanceBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
     

    public static void SetFiledValue(
        object targetInstance,
        string fieldName,
        object value)
    {
        Guard.NotNull(targetInstance);
        Guard.NotNullOrWhiteSpace(fieldName);

        var field = targetInstance.GetType().GetField(fieldName, AllInstanceBindingFlags);
        if (field is null)
        {
            throw new MissingFieldException(
                $"Field '{fieldName}' not found in type '{targetInstance.GetType().FullName}'.");
        }
        field.SetValue(targetInstance, value);
    }


}
