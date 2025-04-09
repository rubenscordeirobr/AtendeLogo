using System.Reflection;
using System.Text.Json;

namespace AtendeLogo.ClientGateway.Common.Helpers;

public static class CommunicationServiceHelper
{
    internal static JsonSerializerOptions? GetJsonOptions<T>()
    {
        var type = typeof(T);
        var attribute = type.GetCustomAttribute<JsonOptionsProviderAttribute>();
        if (attribute is null)
        {
            return null;
        }
        return attribute.GetJsonOptions();
    }
}

