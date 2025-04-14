using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using AtendeLogo.Common.Attributes;

namespace AtendeLogo.Common.Utils;

public static class EnumUtils
{
    public static bool TryParse<TEnum>(
        [NotNullWhen(true)] string? value, out TEnum result)
        where TEnum : struct, Enum
    {
        if (Enum.TryParse(value, true, out TEnum parsedValue) &&
            Enum.IsDefined(parsedValue))
        {
            result = parsedValue;
            return true;
        }
        result = default;
        return false;
    }

    public static TEnum Parse<TEnum>(
        [NotNullWhen(true)] string? value)
        where TEnum : struct, Enum
    {
        if (TryParse(value, out TEnum result))
        {
            return result;
        }
        throw new ArgumentException($"Value '{value}' is not a valid or defined in {typeof(TEnum).Name}.");
    }

    public static bool IsDefined(Type enumType, object value)
    {
        Guard.NotNull(enumType);

        if (!Enum.IsDefined(enumType, value) || value is null)
        {
            return false;
        }

        var member = enumType.GetMember(value!.ToString()!)
            .FirstOrDefault();

        if (member is null)
            return false;

        return member.GetCustomAttribute<UndefinedValueAttribute>() == null;
    }

    public static bool IsDefined<TEnum>(TEnum value)
         where TEnum : struct, Enum
    {
        if (!Enum.IsDefined(value))
        {
            return false;
        }
        var member = typeof(TEnum).GetMember(value!.ToString()!)
            .FirstOrDefault();

        if (member is null)
            return false;

        return member.GetCustomAttribute<UndefinedValueAttribute>() == null;
    }

    public static T Random<T>()
        where T : struct, Enum
    {
        var random = new Random();
        var values = Enum.GetValues<T>();
#pragma warning disable CA5394 
        var randomIndex = random.Next(0, values.Length);
#pragma warning restore CA5394 
        return values[randomIndex];
    }
}
