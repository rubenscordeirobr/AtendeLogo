using AtendeLogo.Application.Contracts.Security;
using AtendeLogo.Infrastructure.Cache;
using AtendeLogo.Infrastructure.Services;
using AtendeLogo.Shared.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace AtendeLogo.Infrastructure;

public static class InfrastructureServiceConfiguration
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton(typeof(IJsonStringLocalizer<>), typeof(JsonStringLocalizer<>));
        
        services.AddSingleton<ISecureConfiguration, SecureConfiguration>();
         
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var redisConnection = configuration.GetConnectionString("CacheRedis");
            return ConnectionMultiplexer.Connect(redisConnection!);
        });

        services.AddSingleton<ISessionCacheService, SessionCacheService>();
        services.AddScoped<IUserSessionService, UserSessionService>();
        services.AddSingleton<ICommandTrackingService, CommandTrackingService>();

        ////create a new instance at time of injection
        services.AddTransient<IEmailSender, EmailSender>();
         
        return services;
    }
}
