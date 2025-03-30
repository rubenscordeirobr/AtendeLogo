using AtendeLogo.ClientGateway.Common;
using AtendeLogo.ClientGateway.Identities;
using AtendeLogo.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace AtendeLogo.ClientGateway;

public static class ClientGatewayServiceConfiguration
{
    public static IServiceCollection AddClientGatewayServices(
        this IServiceCollection services)
    {
        services.AddUserCasesSharedServices();

        services.AddSingleton<IHttpClientResilienceOptions, HttpClientResilienceOptionsDefault>()
            .AddSingleton(typeof(IHttpClientMediator<>), typeof(HttpClientMediator<>))
            .AddTransient<IHttpClientExecutor, HttpClientExecutor>();

        services.AddSingleton<IAdminUserService, AdminUserService>()
            .AddSingleton<IAdminUserAuthenticationService, AdminUserAuthenticationService>()
            .AddSingleton<IAdminUserAuthenticationValidationService, AdminUserAuthenticationValidationService>()
            .AddSingleton<ITenantUserAuthenticationService, TenantUserAuthenticationService>()
            .AddSingleton<ITenantUserAuthenticationValidationService, TenantUserAuthenticationValidationService>()
            .AddSingleton<ITenantService, TenantService>()
            .AddSingleton<ITenantValidationService, TenantValidationService>()
            .AddSingleton<ITenantUserService, TenantUserService>()
            .AddSingleton<ITenantUserValidationService, TenantUserValidationService>();

        return services;
    }
}
