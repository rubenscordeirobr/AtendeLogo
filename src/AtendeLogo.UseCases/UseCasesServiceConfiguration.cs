using System.Reflection;
using AtendeLogo.Application;
using AtendeLogo.UseCases.Identities.Tenants.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AtendeLogo.UseCases;

public static class UseCasesServiceConfiguration
{
    public static IServiceCollection AddUserCasesServices(
        this IServiceCollection services)
    {
        services.AddApplicationHandlersFromAssembly(Assembly.GetExecutingAssembly());
        services.AddScoped<ITenantValidationService, TenantValidationService>();
        return services;
    }

}
