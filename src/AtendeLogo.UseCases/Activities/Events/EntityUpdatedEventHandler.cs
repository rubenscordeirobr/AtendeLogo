using System.Dynamic;
using AtendeLogo.Application.Contracts.Persistence.Activities;
using AtendeLogo.Common.Extensions;
using AtendeLogo.Common.Utils;
using AtendeLogo.Domain.Entities.Activities;
using AtendeLogo.Domain.Primitives;

namespace AtendeLogo.UseCases.Activities.Events;

public sealed class EntityUpdatedEventHandler<TEntity> : IEntityUpdatedEventHandler<TEntity>
    where TEntity : EntityBase
{
    private readonly IActivityRepository _activityRepository;
    private readonly IUserSessionAccessor _userSessionAccessor;
    private readonly ILogger<EntityUpdatedEventHandler<TEntity>> _logger;

    public EntityUpdatedEventHandler(
        IActivityRepository activityRepository,
        IUserSessionAccessor userSessionAccessor,
        ILogger<EntityUpdatedEventHandler<TEntity>> logger)
    {
        _activityRepository = activityRepository;
        _userSessionAccessor = userSessionAccessor;
        _logger = logger;
    }

    public async Task HandleAsync(
        IEntityUpdatedEvent<TEntity> domainEvent)
    {
        var userSession = _userSessionAccessor.GetCurrentSession();
       
        var entity = domainEvent.Entity;
        var properties = domainEvent.ChangedProperties
            .Select(propertyChanged => $"{propertyChanged.PropertyName}: {propertyChanged.PreviousValue} -> {propertyChanged.Value}")
            .ToList();

        var description = $"Updated {entity.GetType().Name} {entity.Id}. Properties: {string.Join(", ", properties)}";
        dynamic newData = new ExpandoObject();
        
        foreach (var property in domainEvent.ChangedProperties)
        {
            ((IDictionary<string, object>)newData)[property.PropertyName] = property.Value ?? "null";
        }

        dynamic oldData = new ExpandoObject();
        foreach (var property in domainEvent.ChangedProperties)
        {
            ((IDictionary<string, object>)oldData)[property.PropertyName] = property.PreviousValue ?? "null";
        }

        var oldDataSerialized = JsonUtils.Serialize(oldData);
        var newDataSerialized = JsonUtils.Serialize(newData);

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
