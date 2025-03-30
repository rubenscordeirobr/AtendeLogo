using System.Reflection;
using AtendeLogo.Application;
using AtendeLogo.UseCases.Identities.Authentications.Services;
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
        services.AddScoped<ITenantUserValidationService, TenantUserValidationService>();
        services.AddScoped<ITenantUserAuthenticationValidationService, TenantUserAuthenticationValidationService>();
        services.AddScoped<IAdminUserAuthenticationValidationService, AdminUserAuthenticationValidationService>();

        return services;
    }

}
