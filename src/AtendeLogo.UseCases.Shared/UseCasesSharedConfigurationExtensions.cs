﻿using System.Reflection;
using AtendeLogo.UseCases.Registrars;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AtendeLogo.UseCases;

public static class UseCasesSharedConfigurationExtensions
{
    public static IServiceCollection AddUserCasesSharedServices(
        this IServiceCollection services,
        IConfiguration configuration)
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
