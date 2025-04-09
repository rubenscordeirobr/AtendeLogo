using AtendeLogo.Application.Models.Secrets;
using AtendeLogo.Common.Exceptions;
using AtendeLogo.Common.Utils;

namespace AtendeLogo.TestCommon.Mocks.Infrastructure;

public static class AzureTranslationSecretsTestFactory
{
    public static AzureTranslationSecrets Create()
    {
        var di = new DirectoryInfo(Directory.GetCurrentDirectory());
        var testDir = di.GetRequiredParent("tests");
        var secretsPath = Path.Combine(testDir.FullName, "../.secrets");
        var secretsFilePath = Path.Combine(secretsPath, "azure-translator-secrets.json");
        return JsonUtils.DeserializeFile<AzureTranslationSecrets>(secretsFilePath)!;
    }
}

