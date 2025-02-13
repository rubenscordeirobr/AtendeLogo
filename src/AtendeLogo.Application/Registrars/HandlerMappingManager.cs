namespace AtendeLogo.Application.Registrars;

internal static class HandlerMappingManager
{
   private readonly static HandlerMappings _domainEventHandlerMappings = new(HandlerKind.DomainEventHandler);
   private readonly static HandlerMappings _domainEventPreProcessorHandlerMappings = new(HandlerKind.PreProcessorHandler);

    internal static void MapperDomainEventHandler(
        Type domainEventType,
        Type handlerType)
    {
        _domainEventHandlerMappings.MapperEventHandler(domainEventType, handlerType);
    }

    internal static void MapperDomainEventPreProcessorHandler(
        Type domainEventType,
        Type handlerType)
    {
        _domainEventPreProcessorHandlerMappings.MapperEventHandler(domainEventType, handlerType);
    }

    internal static IReadOnlyList<Type> GetDomainEventHandlers(Type eventType)
    {
        return _domainEventHandlerMappings.GetHandlerTypes(eventType);
    }

    internal static IReadOnlyList<Type> GetDomainEventPreProcessorHandlers(Type eventType)
    {
        return _domainEventPreProcessorHandlerMappings.GetHandlerTypes(eventType);
    }
}
