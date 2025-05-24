using System.Reflection;
using AtendeLogo.ClientGateway;
using AtendeLogo.ClientGateway.Abstractions;
using AtendeLogo.ClientGateway.Common.Abstractions;
using AtendeLogo.TenantPortal.Components.Pages.Authentication.Login;
using AtendeLogo.TenantPortal.Services;
using AtendeLogo.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace AtendeLogo.TenantPortal;

public static class TenantPortalServiceConfiguration
{
    public static IServiceCollection AddTenantPortalServices(
       this IServiceCollection services)
    {
        services.AddAtendeLogoUIServices()
            .AddTenantUserAuthenticationServices()
            .AddSingleton<IHttpClientProvider, TenantPortalHttpClientProvider>()
            .AddSingleton<IApplicationInfo, TenantPortalApplicationInfo>()
            .AddScoped<IAssetsService, AssetsService>()
            .AddScoped<IClientTenantUserSessionContextService, ClientTenantUserSessionContextService>()
            .AddViewModelsAndValidators(Assembly.GetExecutingAssembly());

        //alias IClientUserSessionContext as IClientTenantUserSessionContext
        services.AddScoped<IClientUserSessionContextService>(serviceProvider =>
        {
            return serviceProvider.GetRequiredService<IClientTenantUserSessionContextService>();
        });
        return services;
    }

    public static async Task InitializeTenantPortalServiceAsync(
         this IServiceProvider serviceProvider, 
         IWebHostEnvironment environment)
    {
        await serviceProvider.InitializeHttpClientProviderAsync(environment);
        await serviceProvider.InitializeUIServiceAsync();
    }

    private static async Task InitializeHttpClientProviderAsync(
        this IServiceProvider serviceProvider, 
        IWebHostEnvironment environment)
    {
        var httpClientProvider = (TenantPortalHttpClientProvider)serviceProvider
           .GetRequiredService<IHttpClientProvider>();

        await httpClientProvider.InitializeAsync(environment);
    }
}
