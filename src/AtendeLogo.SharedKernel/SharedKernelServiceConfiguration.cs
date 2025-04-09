using AtendeLogo.Shared.Abstractions;
using AtendeLogo.Shared.Localization;
using Microsoft.Extensions.DependencyInjection;

namespace AtendeLogo.Shared;

public static class SharedKernelServiceConfiguration
{
    public static IServiceCollection AddSharedKernelServices(
        this IServiceCollection services )
    {
        services
            .AddSingleton<IJsonStringLocalizerCache, JsonStringLocalizerCache>()
            .AddScoped(typeof(IJsonStringLocalizer<>), typeof(JsonStringLocalizer<>));

        return services;
    }
}
