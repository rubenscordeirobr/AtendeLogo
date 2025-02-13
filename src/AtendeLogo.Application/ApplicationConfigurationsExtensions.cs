using System.Reflection;
using AtendeLogo.Application.Mediatores;
using AtendeLogo.Application.Registrars;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AtendeLogo.Application;

public static class ApplicationConfigurationsExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        services.AddScoped<IRequestMediator, RequestMediator>();
        services.AddScoped<IEventMediator, EventMediator>();

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
