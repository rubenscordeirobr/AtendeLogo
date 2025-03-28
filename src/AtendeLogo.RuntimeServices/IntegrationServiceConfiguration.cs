using AtendeLogo.Application.Contracts.Security;
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
            .AddScoped<IRequestMediator, RequestMediator>()
            .AddScoped<IEventMediator, EventMediator>()
            .AddScoped<IUserSessionManager, UserSessionManager>()
            .AddTransient<IUserSessionVerificationService, UserSessionVerificationService>();

        return services;
    }     
}
