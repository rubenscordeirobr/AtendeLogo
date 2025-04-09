using AtendeLogo.Application.Abstractions.Security;
using AtendeLogo.Infrastructure.Cache;
using AtendeLogo.Infrastructure.Configurations;
using AtendeLogo.Infrastructure.Factories;
using AtendeLogo.Infrastructure.Services;
using AtendeLogo.Shared.Abstractions;
using AtendeLogo.Shared.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace AtendeLogo.Infrastructure;

public static class InfrastructureServiceConfiguration
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment hostEnvironment)
    {
        var azureTranslationSecrets = AzureTranslationSecretsFactory.Create(configuration, hostEnvironment);
        var localizationConfiguration = JsonLocalizationConfigurationFactory.Create(configuration, hostEnvironment);

        services.AddSingleton<ISecureConfiguration, SecureConfiguration>()
            .AddSingleton<ICacheRepository, CacheRepository>()
            .AddSingleton<IFileService, FileService>()
            .AddSingleton<JsonLocalizationCacheConfiguration>(localizationConfiguration)
            .AddSingleton(localizationConfiguration)
            .AddSingleton(azureTranslationSecrets);

        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var redisConnection = configuration.GetConnectionString("CacheRedis");
            return ConnectionMultiplexer.Connect(redisConnection!);
        });
        services.AddTransient<IEmailSender, EmailSender>();

        return services;
    }
}
