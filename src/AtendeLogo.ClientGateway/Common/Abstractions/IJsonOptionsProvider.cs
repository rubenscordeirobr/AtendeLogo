using System.Text.Json;

namespace AtendeLogo.ClientGateway.Common.Abstractions;

public interface IJsonOptionsProvider
{
    JsonSerializerOptions GetJsonOptions();
}
