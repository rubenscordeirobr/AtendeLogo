using System.Diagnostics.CodeAnalysis;

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
}
