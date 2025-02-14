using System.Reflection;
using AtendeLogo.Application.Contracts.Handlers;
using AtendeLogo.Application.Contracts.Registrars;
using AtendeLogo.Application.Exceptions;
using AtendeLogo.Common.Extensions;
using AtendeLogo.Common.Helpers;
using AtendeLogo.Domain.Primitives;
using AtendeLogo.Domain.Primitives.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AtendeLogo.Application.Registrars;

internal class ApplicationHandlerRegistrar
{
    private static readonly object _lock = new();
    private readonly IServiceCollection _services;
    private readonly IEventHandlerRegistryService _eventRegistryService;

    public ApplicationHandlerRegistrar(
        IServiceCollection services)
    {
        _services = services;
        _eventRegistryService = InitializeEventHandlerRegistryService();
    }

    private IEventHandlerRegistryService InitializeEventHandlerRegistryService()
    {
        lock (_lock)
        {
            using var temp = _services.BuildServiceProvider();
            var mappingService = temp.GetService<IEventHandlerRegistryService>();
            if (mappingService is not null)
            {
                return mappingService;
            }

            // Check if it's already registered but not yet resolved
            var serviceDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(IEventHandlerRegistryService));
            if (serviceDescriptor is not null)
            {
                throw new InvalidOperationException("The EventHandlerMappingManager is already registered but not yet resolved.");
            }

            _services.AddSingleton<IEventHandlerRegistryService>(new EventHandlerRegistryService());

            return InitializeEventHandlerRegistryService();
        }
    }

    internal void RegisterFromAssembly(Assembly assembly)
    {
        var assemblyTypes = assembly.GetTypes();
        RegisterHandlerFromTypes(assemblyTypes);
    }

    internal void RegisterHandlerFromTypes(Type[] types)
    {
        RegisterRequestHandlers(types);
        RegisterEventHandlers(types);
    }

    private void RegisterRequestHandlers(Type[] assemblyTypes)
    {
        var handlerDefinitionType = typeof(IRequestHandler<,>);
        var handlersTypesMapped = TypeHelper.FindTypesImplementingInterface(assemblyTypes, handlerDefinitionType);

        foreach (var (handlerType, handlerInterfaceType) in handlersTypesMapped)
        {
            CheckIfHasSubclassIn(handlerType, assemblyTypes);
            CheckIfClass(handlerType);

            var requestType = handlerInterfaceType.GetGenericArguments().First();
            var responseType = handlerInterfaceType.GetGenericArguments().Last();

            CheckIfNotGenericType(requestType);
            CheckIfNotGenericType(responseType);

            TypeGuard.MustBeNotGeneric(requestType);
            TypeGuard.MustBeNotGeneric(responseType);

            TypeGuard.MustBeConcrete(responseType);
            TypeGuard.MustBeConcrete(requestType);

            var serviceType = typeof(IRequestHandler<,>)
                .MakeGenericType(requestType, responseType);

            ThrowIfHasAnyOtherHandlerRegistradFor(serviceType, handlerType, requestType, responseType);

            _services.TryAddTransient(serviceType, handlerType);
        }
    }

    private void ThrowIfHasAnyOtherHandlerRegistradFor(
        Type serviceType,
        Type handlerType,
        Type requestType,
        Type responseType)
    {
        var registredServices = _services.Where(x => x.ServiceType == serviceType && x.ImplementationType != handlerType);
        if (registredServices.Any())
        {
            var errorMessage = GetHandlerAlreadyRegistredErrorMessage(
                registredServices,
                handlerType,
                requestType,
                responseType);

            throw new HandlerRegistrationAlreadyExistsException(errorMessage);
        }
    }

    private string GetHandlerAlreadyRegistredErrorMessage(
        IEnumerable<ServiceDescriptor> registredServices,
        Type handlerType,
        Type requestType,
        Type responseType)
    {
        var implementationType = registredServices.First().ImplementationType!;
        return $"The handler type {handlerType.GetQualifiedName()} can't be registrad for " +
               $"the request type {requestType.GetQualifiedName()} and response type {responseType.GetQualifiedName()} " +
               $"because there is already a handler registrad for this request type and response type. " +
               $"The handler {implementationType.GetQualifiedName()} is already registrad for this request type and response type";
    }

    private void RegisterEventHandlers(Type[] assemblyTypes)
    {
        RegisterEventHandlers(
            assemblyTypes,
            typeof(IDomainEventHandler<>),
            HandlerKind.DomainEventHandler,
            _eventRegistryService.MapperDomainEventHandler);

        RegisterEventHandlers(
            assemblyTypes,
            typeof(IPreProcessorHandler<>),
            HandlerKind.PreProcessorHandler,
            _eventRegistryService.MapperDomainEventPreProcessorHandler);
    }

    private void RegisterEventHandlers(Type[] assemblyTypes,
                                       Type handlerDefinition,
                                       HandlerKind eventHandlerType,
                                       Action<Type, Type> mapperEventHadler)
    {
        var handlersTypesMapped = TypeHelper.FindTypesImplementingInterface(assemblyTypes, handlerDefinition);

        foreach (var (handlerType, handlerInterfaceType) in handlersTypesMapped)
        {
            CheckIfHasSubclassIn(handlerType, assemblyTypes);
            CheckIfClass(handlerType);

            var domainEventType = handlerInterfaceType.GetGenericArguments().First();

            ValidateDomainEventType(domainEventType);
            ValidateHandlerType(handlerType, eventHandlerType);

            _services.TryAddTransient(handlerType, handlerType);

            mapperEventHadler.Invoke(domainEventType, handlerType);
        }
    }

    private void CheckIfClass(Type handlerType)
    {
        if (!handlerType.IsClass)
        {
            var message = $"Handler {handlerType.Name} is not a class";
            throw new InvalidOperationException(message);
        }
    }

    private void CheckIfHasSubclassIn(Type handlerType, Type[] assemblyTypes)
    {
        if (handlerType.IsSealed)
            return;

        var subClasse = assemblyTypes.FirstOrDefault(x => x.IsSubclassOf(handlerType));
        if (subClasse is not null)
        {
            var message = $"Handler {handlerType.Name} has a subclass  {subClasse} in the assembly" +
                          "Please, make sure that the handler is not being used as a base class for another or set the handler as abstract class";
            throw new InvalidOperationException(message);
        }
    }

    private void CheckIfNotGenericType(Type type)
    {
        if (type.IsGenericType)
        {
            var message = $"Type {type.Name} is a generic type";
            throw new InvalidOperationException(message);
        }
    }

    private void ValidateDomainEventType(Type eventHandlerType)
    {
        if (eventHandlerType.IsGenericType)
        {
            if (eventHandlerType.GetGenericArguments().Count() > 1)
            {
                var message = $"Event {eventHandlerType.Name} has more than one generic argument";
                throw new NotSupportedException(message);
            }

            if (!eventHandlerType.ImplementsGenericInterfaceDefinition(typeof(IEntityStateChangedEvent<>)))
            {
                var mensagem = $"The generic event {eventHandlerType.Name} does not implement IEntityStateChangedEvent<>";
                throw new NotSupportedException(mensagem);
            }
        }
    }

    private void ValidateHandlerType(Type handlerType, HandlerKind eventHandlerType)
    {
        if (!handlerType.IsClass)
        {
            var message = $"Handler {handlerType.Name} is not a class";
            throw new InvalidOperationException(message);
        }

        if (!handlerType.IsGenericType)
        {
            return;
        }

        if (handlerType.GetGenericArguments().Count() > 1)
        {
            var message = $"Handler {handlerType.Name} has more than one generic argument";
            throw new NotSupportedException(message);
        }

        var genericArguments = handlerType.GetGenericArguments().First();
        if (!genericArguments.IsAssignableTo(typeof(IDomainEvent)) &&
            !genericArguments.IsSubclassOfOrEquals(typeof(EntityBase)))
        {
            var message = $"The generic handler {handlerType.Name} does not implement IDomainEvent";
            throw new NotSupportedException(message);
        }
        if (genericArguments.IsGenericType)
        {
            throw new Exception($"The generic handler {handlerType.Name} has a generic argument");
        }

        if (genericArguments.IsSubclassOfOrEquals(typeof(EntityBase)))
        {
            if (eventHandlerType == HandlerKind.DomainEventHandler)
            {
                if (!handlerType.ImplementsGenericInterfaceDefinition(typeof(IEntityStateChangedEventHandler<>)))
                {
                    var message = $"The generic handler {handlerType.Name} does not implement IEntityStateChangedEventHandler<>";
                    throw new NotSupportedException(message);
                }
            }
            else if (eventHandlerType == HandlerKind.PreProcessorHandler)
            {
                if (!handlerType.ImplementsGenericInterfaceDefinition(typeof(IEntityStateChangedEventPreProcessorHandler<>)))
                {
                    var message = $"The generic handler {handlerType.Name} does not implement IEntityStateChangedEventPreProcessor<>";
                    throw new NotSupportedException(message);
                }
            }
        }
    }
}

internal enum HandlerKind
{
    RequestHandler,
    DomainEventHandler,
    PreProcessorHandler
}
