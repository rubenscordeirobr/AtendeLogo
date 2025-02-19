using AtendeLogo.Presentation.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AtendeLogo.Presentation;

public static class PresentationServiceConfiguration
{
    public static IServiceCollection AddPresentationServices(
        this IServiceCollection services)
    {
        services.AddScoped<IRequestUserSessionService, RequestUserSessionService>();
        return services;
    }
}
