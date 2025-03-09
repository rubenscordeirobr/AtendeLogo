using System.Reflection;
using AtendeLogo.Application.Contracts.Services;
using AtendeLogo.Application.Mediatores;
using AtendeLogo.Application.Registrars;
using AtendeLogo.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AtendeLogo.Application;

public static class ApplicationServiceConfiguration
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services )
    {

        services.AddScoped<IRequestMediator, RequestMediator>();
        services.AddScoped<IEventMediator, EventMediator>();

        services.AddSingleton<ICommandTrackingService, CommandTrackingService>();
        services.AddSingleton<ISessionCacheService, SessionCacheService>();
        services.AddTransient<IUserSessionVerificationService, UserSessionVerificationService>();

        
        services.AddApplicationHandlersFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }

    public static IServiceCollection AddApplicationHandlersFromAssembly(
       this IServiceCollection services,
       Assembly assembly)
    {
        var serviceRegistrar = new ApplicationHandlerRegistrar(services);
        serviceRegistrar.RegisterFromAssembly(assembly);
        return services;
    }

    internal static IServiceCollection AddApplicationHandlersFromTypes(
       this IServiceCollection services,
       Type[] handlerTypes)
    {
        var serviceRegistrar = new ApplicationHandlerRegistrar(services);
        serviceRegistrar.RegisterHandlerFromTypes(handlerTypes);
        return services;
    }
}
