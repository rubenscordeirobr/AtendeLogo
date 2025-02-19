using System.Dynamic;
using System.Text.Json;
using AtendeLogo.Application.Contracts.Persistence.Activities;
using AtendeLogo.Common.Extensions;
using AtendeLogo.Domain.Entities.Activities;
using AtendeLogo.Domain.Primitives;

namespace AtendeLogo.UseCases.Activities.Events;

public sealed class EntityUpdatedEventHandler<TEntity> : IEntityUpdatedEventHandler<TEntity>
    where TEntity : EntityBase
{
    private readonly IActivityRepository _activityRepository;
    private readonly IRequestUserSessionService _userSessionService;
    private readonly ILogger<EntityUpdatedEventHandler<TEntity>> _logger;

    public EntityUpdatedEventHandler(
        IActivityRepository activityRepository,
        IRequestUserSessionService userSessionService,
        ILogger<EntityUpdatedEventHandler<TEntity>> logger)
    {
        _activityRepository = activityRepository;
        _userSessionService = userSessionService;
        _logger = logger;
    }

    public async Task HandleAsync(
        IEntityUpdatedEvent<TEntity> domainEvent)
    {
        var userSession = _userSessionService.GetCurrentSession();
       
        var entity = domainEvent.Entity;
        var properties = domainEvent.ChangedProperties
            .Select(propertyChanged => $"{propertyChanged.PropertyName}: {propertyChanged.OldValue} -> {propertyChanged.NewValue}")
            .ToList();

        var description = $"Updated {entity.GetType().Name} {entity.Id}. Properties: {string.Join(", ", properties)}";
        dynamic newData = new ExpandoObject();
        
        foreach (var property in domainEvent.ChangedProperties)
        {
            ((IDictionary<string, object>)newData)[property.PropertyName] = property.NewValue ?? "null";
        }

        dynamic oldData = new ExpandoObject();
        foreach (var property in domainEvent.ChangedProperties)
        {
            ((IDictionary<string, object>)oldData)[property.PropertyName] = property.OldValue ?? "null";
        }

        var oldDataSerialized = JsonSerializer.Serialize(oldData);
        var newDataSerialized = JsonSerializer.Serialize(newData);

        var qualifiedTypeName = entity.GetType().GetQualifiedName();

        var activity = new UpdatedActivity
        {
            Tenant_Id = userSession.Tenant_Id,
            UserSession_Id = userSession.Id,
            ActivityDate = DateTime.UtcNow,
            Description = description,
            OldData = oldDataSerialized,
            NewData = newDataSerialized,
            QualifiedTypeName = qualifiedTypeName,
            Entity_Id = entity.Id,
        };

        try
        {
            await _activityRepository.AddAsync(activity);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error on add activity for entity {EntityType} {EntityId}", entity.GetType().Name, entity.Id);
            throw;
        }
    }
}
