using AtendeLogo.ClientGateway;
using AtendeLogo.ClientGateway.Common.Abstractions;
using AtendeLogo.UI.Services;
using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;

namespace AtendeLogo.UI;

public static class UIServiceConfiguration
{
    public static IServiceCollection AddAtendeLogoUIServices(
       this IServiceCollection services)
    {
        return services.AddCascadingAuthenticationState()
            .AddFluentUIComponents()
            .AddBlazoredLocalStorage()
            .AddClientGatewayServices()
            .AddScoped<IInternetStatusService, InternetStatusService>()
            .AddScoped<IConnectionStatusNotifier, ConnectionStatusNotifier>()
            .AddScoped<IRequestErrorNotifier, RequestErrorNotifier>()
            .AddScoped<ICultureProvider, CultureProvider>()
            .AddScoped<IClientAuthorizationTokenManager, ClientAuthorizationTokenManager>()
            .AddScoped<IBusyIndicatorService, BusyIndicatorService>()
            .AddScoped<IThemeService, ThemeService>();
    }

    public static async Task InitializeUIServiceAsync(
       this IServiceProvider serviceProvider)
    {
        await serviceProvider.InitializeClientServiceAsync();
    }
        
}
