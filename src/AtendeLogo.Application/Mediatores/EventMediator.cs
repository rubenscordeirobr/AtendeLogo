using AtendeLogo.Application.Contracts.Handlers;
using AtendeLogo.Application.Contracts.Registrars;
using AtendeLogo.Application.Events;
using AtendeLogo.Application.Exceptions;
using AtendeLogo.Common.Extensions;
using AtendeLogo.Domain.Primitives.Contracts;
using Microsoft.Extensions.Logging;

namespace AtendeLogo.Application.Mediatores;

internal partial class EventMediator : IEventMediator
{
    private readonly IEventHandlerRegistryService _eventHandlerRegistryService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<EventMediator> _logger;

    public EventMediator(
        IServiceProvider serviceProvider,
        IEventHandlerRegistryService eventHandlerRegistryService,
        ILogger<EventMediator> logger)
    {
        _eventHandlerRegistryService = eventHandlerRegistryService;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task PreProcessorDispatchAsync(
        IDomainEventContext eventContext,
        CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in eventContext.Events)
        {
            var handlerTypes = _eventHandlerRegistryService.GetDomainEventPreProcessorHandlers(domainEvent.GetType());
            if (handlerTypes.Any())
            {
                var results = new List<ExecutedDomainEventResult>();
                var eventData = CreateEventData(eventContext, domainEvent, results);
                if (eventData != null)
                {
                    foreach (var handlerType in handlerTypes)
                    {
                        var result = await ExecuteHandleAsync(domainEvent, handlerType, eventData);
                        results.Add(result);

                        if (eventContext.IsCanceled || cancellationToken.IsCancellationRequested)
                        {
                            return;
                        }
                    }
                }
                eventContext.AddExecutedEventResults(domainEvent, results);
            }
        }
    }

    private object? CreateEventData(IDomainEventContext eventContext, IDomainEvent domainEvent, List<ExecutedDomainEventResult> results)
    {
        try
        {
            return DomainEventDataFactory.Create(eventContext, domainEvent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error on create event data for event {EventType}", domainEvent.GetType());

            var exception = new InvalidOperationException($"Error on create event data for event {domainEvent.GetType()}", ex);
            results.Add(new ExecutedDomainEventResult(
                DomainEvent: domainEvent,
                HandlerType: typeof(DomainEventDataFactory),
                ImplementationHandlerType: null,
                Handler: null,
                Exception: exception
            ));
            return null;
        }
    }

    public async Task DispatchAsync(IDomainEventContext eventContext)
    {
        if (eventContext.IsCanceled)
        {
            throw new DomainEventContextCancelledException();
        }

        foreach (var domainEvent in eventContext.Events)
        {
            var handlerTypes = _eventHandlerRegistryService.GetDomainEventHandlers(domainEvent.GetType());
            var results = new List<ExecutedDomainEventResult>();

            foreach (var handlerType in handlerTypes)
            {
                var result = await ExecuteHandleAsync(domainEvent, handlerType, domainEvent);
                results.Add(result);
            }
            eventContext.AddExecutedEventResults(domainEvent, results);
        }
    }

    private IApplicationHandler GetHandler(IDomainEvent domainEvent, Type handlerType)
    {
        handlerType = NormalizeHandlerType(domainEvent, handlerType);
        var handler = _serviceProvider.GetService(handlerType);
        if (handler == null)
        {
            throw new InvalidOperationException($"Handler {handlerType} not found for {domainEvent.GetType()}");
        }

        if (handler is IApplicationHandler domainEventHandler)
        {
            return domainEventHandler;
        }
        throw new InvalidOperationException($"Handler {handlerType} not found for {domainEvent.GetType()}");
    }

    private async Task<ExecutedDomainEventResult> ExecuteHandleAsync(
        IDomainEvent domainEvent,
        Type handlerType,
        object handlerData)
    {
        Exception? exception = null;
        IApplicationHandler? handler = null;

        try
        {
            handler = GetHandler(domainEvent, handlerType);
            await handler.HandleAsync(handlerData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error on handle event {EventType} with handler {handlerType}",
                domainEvent.GetType(),
                handlerType);

            exception = ex;
        }

        return new ExecutedDomainEventResult(
               DomainEvent: domainEvent,
               HandlerType: handlerType,
               ImplementationHandlerType: handler?.GetType(),
               Handler: handler,
               Exception: exception
        );
    }

    private Type NormalizeHandlerType(IDomainEvent domainEvent, Type handlerType)
    {
        if (handlerType.IsGenericType)
        {
            if (handlerType.ContainsGenericParameters)
            {
                if (domainEvent is IEntityStateChangedEvent entityStateChangedEvent)
                {
                    var isEntityStateEventHandlerType = handlerType
                        .ImplementsGenericInterfaceDefinition(typeof(IEntityStateChangedEventHandler<>)) ||
                        handlerType.ImplementsGenericInterfaceDefinition(typeof(IEntityStateChangedEventPreProcessorHandler<>));

                    if (isEntityStateEventHandlerType)
                    {
                        return handlerType.MakeGenericType(entityStateChangedEvent.EntityBase.GetType());
                    }

                    throw new InvalidOperationException($"Event {domainEvent.GetType().Name} not implements  {nameof(IEntityStateChangedEvent)}");
                }
                throw new InvalidOperationException($"Event {domainEvent.GetType().Name} has generic arguments with ContainsGenericParameters");
            }
        }
        return handlerType;

    }
}

