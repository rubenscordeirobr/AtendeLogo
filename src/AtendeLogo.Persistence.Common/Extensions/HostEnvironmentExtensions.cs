using Microsoft.Extensions.Hosting;

namespace AtendeLogo.Persistence.Common.Extensions;

public static class HostEnvironmentExtensions
{
    public static bool IsDockerCompose(this IHostEnvironment hostEnvironment)
    {
        Guard.NotNull(hostEnvironment);
        if (hostEnvironment.IsDevelopment())
        {
            return false;
        }

        var subEnv = Environment.GetEnvironmentVariable("ASPNETCORE_SUB_ENVIRONMENT");
      
        return string.Equals(
            subEnv,
            "DockerCompose",
            StringComparison.OrdinalIgnoreCase);

    }

    public static bool IsTest(this IHostEnvironment hostEnvironment)
    {
        Guard.NotNull(hostEnvironment);
        if (!hostEnvironment.IsDevelopment())
        {
            return false;
        }

        var subEnv = Environment.GetEnvironmentVariable("ASPNETCORE_SUB_ENVIRONMENT");
        return string.Equals(
            subEnv,
            "Test",
            StringComparison.OrdinalIgnoreCase);
    }

    public static bool IsAspire(this IHostEnvironment hostEnvironment)
    {
        Guard.NotNull(hostEnvironment);
        if (!hostEnvironment.IsDevelopment())
        {
            return false;
        }

        var subEnv = Environment.GetEnvironmentVariable("ASPNETCORE_SUB_ENVIRONMENT");
        return string.Equals(
            subEnv,
            "Aspire",
            StringComparison.OrdinalIgnoreCase);

    }
}
