using AtendeLogo.Application.Abstractions.Security;
using AtendeLogo.RuntimeServices.Providers;
using AtendeLogo.RuntimeServices.Services.Azure;
using Microsoft.Extensions.DependencyInjection;

namespace AtendeLogo.RuntimeServices;

public static class RuntimeServicesConfiguration
{
    public static IServiceCollection AddRuntimeServices(
        this IServiceCollection services)
    {
        services.AddSingleton<ICommandTrackingService, CommandTrackingService>()
            .AddSingleton<IUserSessionCacheService, UserSessionCacheService>()
            .AddSingleton<IUserSessionTokenHandler, UserSessionTokenHandler>()
            .AddSingleton<IEntityAuthorizationService, EntityAuthorizationService>()
            .AddSingleton<IAuthenticationAttemptLimiterService, AuthenticationAttemptLimiterService>()
            .AddSingleton<IJsonStringLocalizerService, JsonStringLocalizerService>()
            .AddSingleton<ITranslationService, AzureTranslationService>()
            .AddScoped<IRequestMediator, RequestMediator>()
            .AddScoped<IEventMediator, EventMediator>()
            .AddScoped<IUserSessionManager, UserSessionManager>()
            .AddScoped<ICultureProvider, CultureProvider>()
            .AddTransient<IUserSessionVerificationService, UserSessionVerificationService>();

        return services;
    }     
}
