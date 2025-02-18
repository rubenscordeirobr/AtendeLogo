using AtendeLogo.Application.Contracts.Events;
using AtendeLogo.Application.Events;
using AtendeLogo.Domain.Primitives.Contracts;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AtendeLogo.Persistence.Common.UnitOfWorks;

internal class DomainEventContextFactory
{
    internal static DomainEventContext Create(
        UserSession userSession,
        IEnumerable<EntityEntry<EntityBase>> entries)
    {
        var aggregateEvents = entries
            .Select(e => e.Entity as IEventAggregate)
            .Where(e => e is not null)
            .Cast<IEventAggregate>()
            .SelectMany(x => x.DomainEvents)
            .ToList();

        var entityChangeEvents = entries
            .Where(entry => entry is not null)
            .Select(entry => CreateEntityStateChangedEven(entry))
            .ToList();

        var allEvents = aggregateEvents
            .Concat(entityChangeEvents)
            .ToList();

        return new DomainEventContext(allEvents);
    }

    private static IEntityStateChangedEvent CreateEntityStateChangedEven(
        EntityEntry<EntityBase> entry)
    {
        return entry.State switch
        {
            EntityState.Added => CreateEvent<IPropertyValueEvent>(entry, typeof(EntityCreatedEvent<>)),
            EntityState.Deleted => CreateEvent<IPropertyValueEvent>(entry, typeof(EntityDeletedEvent<>)),
            EntityState.Modified => CreateEvent<IChangedPropertyEvent>(entry, typeof(EntityUpdatedEvent<>)),
            _ => throw new InvalidOperationException("Failed to create entity state changed event. Invalid entity state.")
        };
    }

    private static IEntityStateChangedEvent CreateEvent<TPropertyEvent>(
        EntityEntry<EntityBase> entry,
        Type entityStateEventType)
        where TPropertyEvent : IPropertyEvent
    {
        
        var entity = entry.Entity;
        var entityType = entity.GetType();
        var eventType = entityStateEventType.MakeGenericType(entityType);
        var propertyEvents = GetPropertyEvents<TPropertyEvent>(entry);
        var parameters = new object[] { entity, propertyEvents };

        var entityChangedEvent = Activator.CreateInstance(eventType, parameters);

        if (entityChangedEvent is null)
            throw new InvalidOperationException("Failed to create entity state changed event.");

        if (entityChangedEvent is IEntityStateChangedEvent entityStateChangedEvent)
            return entityStateChangedEvent;

        throw new InvalidOperationException("Failed to create entity state changed event. Invalid event type.");
    }

    private static IReadOnlyList<TPropertyEvent> GetPropertyEvents<TPropertyEvent>(
        EntityEntry<EntityBase> entry)
        where TPropertyEvent : IPropertyEvent
    {
        var propertyEvents = new List<TPropertyEvent>();
        foreach (var property in entry.Properties)
        {
            var propertyEvent = CreatePropertyEvent(property, entry.State);
            if (propertyEvent is not null)
            {
                propertyEvents.Add((TPropertyEvent)propertyEvent);
            }
        }
        return propertyEvents;
    }

    private static IPropertyEvent? CreatePropertyEvent(
        PropertyEntry property,
        EntityState state)
    {
        return state switch
        {
            EntityState.Modified => CreateChangedPropertyEvent(property),
            EntityState.Added | EntityState.Detached => CreatePropertyValueEvent(property),
            _ => null
        };
    }

    private static IPropertyEvent? CreateChangedPropertyEvent(PropertyEntry property)
    {
        if (!property.IsModified)
            return null;

        var originalValue = property.OriginalValue;
        var currentValue = property.CurrentValue;

        if (EqualityComparer<object>.Default.Equals(originalValue, currentValue))
            return null;

        return new ChangedPropertyEvent(
            PropertyName: property.Metadata.Name,
            OldValue: originalValue,
            NewValue: currentValue
         );
    }

    private static IPropertyEvent CreatePropertyValueEvent(PropertyEntry property)
    {
        return new PropertyValueEvent(
            PropertyName: property.Metadata.Name,
            Value: property.CurrentValue
        );
    }
}
