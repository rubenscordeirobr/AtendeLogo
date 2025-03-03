﻿using AtendeLogo.Domain.Entities.Activities;
using System.Dynamic;
using System.Text.Json;
using AtendeLogo.Domain.Primitives;
using AtendeLogo.Common.Extensions;
using AtendeLogo.Application.Contracts.Persistence.Activities;

namespace AtendeLogo.UseCases.Activities.Events;

public sealed class EntityDeletedEventHandler<TEntity> : IEntityDeletedEventHandler<TEntity>
    where TEntity : EntityBase
{
    private readonly IActivityRepository _activityRepository;
    private readonly IUserSessionAccessor _userSessionAccessor;
    private readonly ILogger<EntityDeletedEventHandler<TEntity>> _logger;

    public EntityDeletedEventHandler(
        IActivityRepository activityRepository,
        IUserSessionAccessor userSessionAccessor,
        ILogger<EntityDeletedEventHandler<TEntity>> logger )
    {
        _activityRepository = activityRepository;
        _userSessionAccessor = userSessionAccessor;
        _logger = logger;
    }

    public async Task HandleAsync(
        IEntityDeletedEvent<TEntity> domainEvent)
    {
        var userSession = _userSessionAccessor.GetCurrentSession();
        var entity = domainEvent.Entity;

        var properties = domainEvent.PropertyValues
            .Select(propertyChanged => $"{propertyChanged.PropertyName}: {propertyChanged.Value}")
            .ToList();

        var description = $"Deleted {entity.GetType().Name} {entity.Id}. Properties: {string.Join(", ", properties)}";
        dynamic data = new ExpandoObject();
        foreach (var property in domainEvent.PropertyValues)
        {
            ((IDictionary<string, object>)data)[property.PropertyName] = property.Value ?? "null";
        }

        var dataSerialized = JsonSerializer.Serialize(data);
        var qualifiedTypeName = entity.GetType().GetQualifiedName();

        var activity = new CreatedActivity
        {
            Tenant_Id = userSession.Tenant_Id,
            UserSession_Id = userSession.Id,
            ActivityDate = DateTime.UtcNow,
            Description = description,
            CreatedData = dataSerialized,
            QualifiedTypeName = qualifiedTypeName,
            Entity_Id = entity.Id,
        };

        try
        {
            await _activityRepository.AddAsync(activity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error on add activity for entity {EntityType} {EntityId}", entity.GetType().Name, entity.Id);
            throw;
        }

    }
}
