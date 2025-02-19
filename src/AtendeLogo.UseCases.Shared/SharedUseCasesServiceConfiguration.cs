﻿using System.Reflection;
using AtendeLogo.UseCases.Common.Registrars;
using Microsoft.Extensions.DependencyInjection;

namespace AtendeLogo.UseCases;

public static class SharedUseCasesServiceConfiguration
{
    public static IServiceCollection AddUserCasesSharedServices(
        this IServiceCollection services )
    {
        services.AddCommandValidationServicesFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }

    public static IServiceCollection AddCommandValidationServicesFromAssembly(
       this IServiceCollection services,
       Assembly assembly)
    {
        var validationRegistrar = new CommandValidationRegistrar(services);
        validationRegistrar.RegisterFromAssembly(assembly);
        return services;
    }

    internal static IServiceCollection AddCommandValidationServicesFromTypes(
        this IServiceCollection services,
        Type[] types)
    {
        var validationRegistrar = new CommandValidationRegistrar(services);
        validationRegistrar.RegisterFromTypes(types);
        return services;
    }
}
