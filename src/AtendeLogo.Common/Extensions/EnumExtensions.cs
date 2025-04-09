using System.ComponentModel;
using System.Reflection;

namespace AtendeLogo.Common.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        Guard.NotNull(value);

        var field = value.GetType()
            .GetField(value.ToString());

        if (field is null)
            return string.Empty;

        var attribute = field.GetCustomAttribute<DescriptionAttribute>();
        return attribute?.Description ?? string.Empty;
    }
}

