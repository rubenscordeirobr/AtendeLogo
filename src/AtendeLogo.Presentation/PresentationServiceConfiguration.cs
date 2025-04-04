using AtendeLogo.Application.Extensions;
using AtendeLogo.Presentation.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AtendeLogo.Presentation;

public static class PresentationServiceConfiguration
{
    public static IServiceCollection AddPresentationServices(
        this IServiceCollection services)
    {
        services.AddScoped<IHttpContextSessionAccessor, HttpContextSessionAccessor>()
            .AddTransient(provider =>
            {
                var accessor = provider.GetRequiredService<IHttpContextSessionAccessor>();
                return accessor.GetRequiredUserSession();
            });
        return services;
    }
}
