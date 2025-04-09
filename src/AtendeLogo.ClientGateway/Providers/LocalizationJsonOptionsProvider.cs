using System.Text.Json;
using AtendeLogo.Common.Utils;

namespace AtendeLogo.ClientGateway.Providers;

public class LocalizationJsonOptionsProvider : IJsonOptionsProvider
{
    public JsonSerializerOptions GetJsonOptions()
    {
        return JsonUtils.LocalizationJsonSerializerOptions;
    }
}
