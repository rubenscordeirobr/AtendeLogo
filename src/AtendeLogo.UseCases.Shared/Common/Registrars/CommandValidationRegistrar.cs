﻿using System.Reflection;
using AtendeLogo.Common.Extensions;
using AtendeLogo.Common.Helpers;
using AtendeLogo.UseCases.Common.Excpetions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AtendeLogo.UseCases.Common.Registrars;

internal class CommandValidationRegistrar
{
    private readonly IServiceCollection _services;

    internal CommandValidationRegistrar(
        IServiceCollection services )
    {
        _services = services;
    }

    internal void RegisterFromAssembly(Assembly assembly)
    {
        var assemblyTypes = assembly.GetTypes();
        RegisterFromTypes(assemblyTypes);
    }

    internal void RegisterFromTypes(Type[] types)
    {
        var mappedTypes = TypeHelper.FindTypesImplementingInterface(
            types,
            typeof(IValidator<>));

        foreach (var (validationType, interfaceType) in mappedTypes)
        {
            var commandType = interfaceType.GetGenericArguments().First();
            if (validationType is null)
            {
                throw new InvalidOperationException($"Validation not found for command: {commandType.Name}");
            }

            var serviceType = typeof(IValidator<>).MakeGenericType(commandType);
            if (commandType.IsSubclassOf<ValueObjectBase>())
            {
                continue;
            }

            ThrowIfHandAnyOtherCommandValidatorRegistredFor(
                commandType,
                serviceType,
                validationType);

            _services.TryAddTransient(serviceType, validationType);
        }
    }

    private void ThrowIfHandAnyOtherCommandValidatorRegistredFor(
        Type commandType,
        Type serviceType,
        Type validationType)
    {
        var registredValidator = _services.Where(
            service => service.ServiceType == serviceType &&
                        service.ImplementationType != validationType);

        if (registredValidator.Any())
        {
            var erroMessage = GetErrorMessage(registredValidator, commandType, serviceType, validationType);
            throw new CommandValidatorAlreadyExistsException(erroMessage);
        }
    }

    private string GetErrorMessage(IEnumerable<ServiceDescriptor> registredValidator,
        Type commandType,
        Type serviceType,
        Type validationType)
    {
        var implementationType = registredValidator.First().ImplementationType!;
        return $" The command {commandType.GetQualifiedName()} can't be registered for " +
               $"the validator {validationType.GetQualifiedName()} because it is already registered for " +
               $"{implementationType.GetQualifiedName()}";
    }
}
