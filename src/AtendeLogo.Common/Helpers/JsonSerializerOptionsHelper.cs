using System.Text.Json;

namespace AtendeLogo.Common.Helpers;

public class JsonSerializerOptionsHelper
{
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
}
