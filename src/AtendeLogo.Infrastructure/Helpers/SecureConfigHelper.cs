using AtendeLogo.Common.Utils;

namespace AtendeLogo.Infrastructure.Helpers;

internal static class SecureConfigHelper
{
    public static string BuildConfigurationKey(string key)
       => $"AppSettings:{key}";

    public static string BuildEnvironmentKey(string key)
        => "ATENDE_LOGO_" + CaseConventionUtils.ToScreamingSnakeCase(key);
}

