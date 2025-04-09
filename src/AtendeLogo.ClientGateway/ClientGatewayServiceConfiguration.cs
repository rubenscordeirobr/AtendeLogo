using AtendeLogo.ClientGateway.Common;
using AtendeLogo.ClientGateway.Identities;
using AtendeLogo.Common.Enums;
using AtendeLogo.Common.Helpers;
using AtendeLogo.Shared.Abstractions;
using AtendeLogo.Shared.Extensions;
using AtendeLogo.Shared.Localization;
using AtendeLogo.UseCases;
using AtendeLogo.UseCases.Identities.Authentications.Commands;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AtendeLogo.ClientGateway;

public static class ClientGatewayServiceConfiguration
{
    public static IServiceCollection AddClientGatewayServices(
        this IServiceCollection services)
    {
        var isDevelopment = EnvironmentHelper.IsDevelopment();
        var localizerConfiguration = new JsonLocalizationCacheConfiguration
        {
            AutoAddMissingKeys = isDevelopment,
            AutoUpdateDefaultKeys = isDevelopment,
            AutoTranslate = isDevelopment,
            CustomTranslationModelId = null
        };

        services.AddSharedKernelServices()
            .AddUserCasesSharedServices();

        services.AddSingleton<IHttpClientResilienceOptions, HttpClientResilienceOptionsDefault>()
            .AddSingleton(localizerConfiguration);

        services.AddScoped<ITenantService, TenantService>()
            .AddScoped<ITenantValidationService, TenantValidationService>()
            .AddScoped<ITenantUserService, TenantUserService>()
            .AddScoped<ITenantUserValidationService, TenantUserValidationService>()
            .AddScoped<IJsonStringLocalizerService, JsonStringLocalizerService>()
            .AddScoped(typeof(IHttpClientMediator<>), typeof(HttpClientMediator<>))
            .AddTransient<IHttpClientExecutor, HttpClientExecutor>();

        services.Remove<IValidator<TenantUserLoginCommand>>();
        services.Remove<IValidator<TenantUserLogoutCommand>>();

        services.Remove<IValidator<AdminUserLoginCommand>>();
        services.Remove<IValidator<AdminUserLogoutCommand>>();

        return services;
    }

    public static IServiceCollection AddTenantUserAuthenticationServices(
       this IServiceCollection services)
    {
        return services.AddScoped<ITenantUserAuthenticationService, TenantUserAuthenticationService>()
            .AddScoped<ITenantUserAuthenticationValidationService, TenantUserAuthenticationValidationService>()
            .AddScoped<IValidator<TenantUserLoginCommand>, TenantUserLoginCommandValidator>()
            .AddScoped<IValidator<TenantUserLogoutCommand>, TenantUserLogoutCommandValidator>();
    }

    public static IServiceCollection AddAdminUserAuthenticationServices(
       this IServiceCollection services)
    {
        return services.AddScoped<IAdminUserAuthenticationService, AdminUserAuthenticationService>()
            .AddScoped<IAdminUserAuthenticationValidationService, AdminUserAuthenticationValidationService>()
            .AddScoped<IValidator<AdminUserLoginCommand>, AdminUserLoginCommandValidator>()
            .AddScoped<IValidator<AdminUserLogoutCommand>, AdminUserLogoutCommandValidator>()
            .AddScoped<IAdminUserService, AdminUserService>();
    }

    public static async Task InitializeClientServiceAsync(
      this IServiceProvider serviceProvider)
    {
        var cache = serviceProvider.GetRequiredService<IJsonStringLocalizerCache>();
        await cache.InitializeAsync(Language.Default);
    }
}
