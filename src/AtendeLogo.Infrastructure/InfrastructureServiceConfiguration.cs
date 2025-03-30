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
        services.AddSingleton(typeof(IJsonStringLocalizer<>), typeof(JsonStringLocalizer<>))
            .AddSingleton<ISecureConfiguration, SecureConfiguration>()
            .AddSingleton<ICacheRepository, CacheRepository>();

        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var redisConnection = configuration.GetConnectionString("CacheRedis");
            return ConnectionMultiplexer.Connect(redisConnection!);
        });

        
        services.AddTransient<IEmailSender, EmailSender>();
         
        return services;
    }
}
