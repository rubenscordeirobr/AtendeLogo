using System.Collections;
using System.Text.Json;

namespace AtendeLogo.Common.Utils;

public class JsonUtils
{
    public static string Serialize(object? obj, JsonSerializerOptions? options = null)
    {
        options ??= JsonSerializerOptions.Web;
        EnableIndentationInDevelopment(options);
        return JsonSerializer.Serialize(obj, options ?? JsonSerializerOptions.Web);
    }

    public static T? Deserialize<T>(string json, JsonSerializerOptions? options = null)
    {
        return JsonSerializer.Deserialize<T>(json, options ?? JsonSerializerOptions.Web);
    }

    public static object? Deserialize(string json, Type returnType, JsonSerializerOptions? options = null)
    {
        return JsonSerializer.Deserialize(json, returnType: returnType, options ?? JsonSerializerOptions.Web);
    }

    public static void EnableIndentationInDevelopment(JsonSerializerOptions jsonSerializerOptions)
    {
        Guard.NotNull(jsonSerializerOptions);

        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
        {
            return;
        }

        if (jsonSerializerOptions.WriteIndented)
            return;

        jsonSerializerOptions.WriteIndented = true;
    }

    public static string GetJsonSchemaType(Type type)
    {
        type = Nullable.GetUnderlyingType(type) ?? type;

        if (type == typeof(string))
        {
            return "string";
        }

        if (type == typeof(bool))
        {
            return "boolean";
        }

        if (type == typeof(int) || type == typeof(long) || type == typeof(short) ||
            type == typeof(byte) || type == typeof(uint) || type == typeof(ulong) || type == typeof(ushort))
        {
            return "integer";
        }

        if (type == typeof(float) || type == typeof(double) || type == typeof(decimal))
        {
            return "number";
        }

        if (type.IsEnum)
        {
            return "number";
        }

        if (typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string))
        {
            return "array";
        }
        return "object";
    }
}

