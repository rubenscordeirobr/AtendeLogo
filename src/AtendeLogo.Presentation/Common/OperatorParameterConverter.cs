using System.Globalization;

namespace AtendeLogo.Presentation.Common;

public static class OperatorParameterConverter
{
    public static string? ToString(object? value, Type type)
    {
        var isNullable = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        if (value is null)
        {
            return null;
        }

        type = type.GetUnderlyingType();

        if (type == typeof(double))
        {
            return ((double)value).ToString(CultureInfo.InvariantCulture);
        }
        if (type == typeof(float))
        {
            return ((float)value).ToString(CultureInfo.InvariantCulture);
        }
        if (type == typeof(decimal))
        {
            return ((decimal)value).ToString(CultureInfo.InvariantCulture);
        }
        if (type == typeof(DateTime))
        {
            return ((DateTime)value).ToString("O", CultureInfo.InvariantCulture);
        }
        if (type == typeof(TimeSpan))
        {
            return ((TimeSpan)value).ToString("c", CultureInfo.InvariantCulture);
        }
        return Convert.ToString(value, CultureInfo.InvariantCulture);
    }

    public static object? Parse(string? value, Type type)
    {
        var isNullable = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        
        if (string.IsNullOrEmpty(value) || value == "null")
        {
            return isNullable 
                ? null
                : Activator.CreateInstance(type);
        }

        if (isNullable)
        {
            type = type.GetUnderlyingType();
        }
        
        if (type == typeof(bool))
        {
            return bool.Parse(value);
        }
        if (type == typeof(int))
        {
            return int.Parse(value, CultureInfo.InvariantCulture);
        }
        if (type == typeof(long))
        {
            return long.Parse(value, CultureInfo.InvariantCulture);
        }
        if (type == typeof(short))
        {
            return short.Parse(value, CultureInfo.InvariantCulture);
        }
        if (type == typeof(string))
        {
            return value;
        }
        if (type == typeof(double))
        {
            return double.Parse(value, CultureInfo.InvariantCulture);
        }
        if (type == typeof(float))
        {
            return float.Parse(value, CultureInfo.InvariantCulture);
        }
        if (type == typeof(decimal))
        {
            return decimal.Parse(value, CultureInfo.InvariantCulture);
        }
        if (type == typeof(Guid))
        {
            return Guid.Parse(value);
        }

        if (type == typeof(DateTime))
        {
            return DateTime.Parse(value, CultureInfo.InvariantCulture);
        }
        if (type == typeof(TimeSpan))
        {
            return TimeSpan.Parse(value, CultureInfo.InvariantCulture);
        }

        if (type.IsEnum)
        {
            return Enum.Parse(type, value, ignoreCase: true);
        }

        // Fallback to ChangeType for other convertible types.
        return Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
    }
}
