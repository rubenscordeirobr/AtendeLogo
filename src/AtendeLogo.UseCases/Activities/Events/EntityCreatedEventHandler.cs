using AtendeLogo.Domain.Entities.Activities;
using System.Dynamic;
using System.Text.Json;
using AtendeLogo.Domain.Primitives;
using AtendeLogo.Common.Extensions;
using AtendeLogo.Application.Contracts.Persistence.Activities;

namespace AtendeLogo.UseCases.Activities.Events;

public sealed class EntityCreatedEventHandler<TEntity> : IEntityCreatedEventHandler<TEntity>
    where TEntity : EntityBase
{
    private readonly IActivityRepository _activityRepository;
    private readonly IUserSessionService _userSessionService;
    private readonly ILogger<EntityCreatedEventHandler<TEntity>> _logger;

    public EntityCreatedEventHandler(
        IActivityRepository activityRepository,
        IUserSessionService userSessionService,
        ILogger<EntityCreatedEventHandler<TEntity>> logger)
    {
        _activityRepository = activityRepository;
        _userSessionService = userSessionService;
        _logger = logger;
    }

    public async Task HandleAsync(IEntityCreatedEvent<TEntity> domainEvent)
    {
        var userSession = _userSessionService.GetCurrentSession();
        var entity = domainEvent.Entity;
       
        var properties = domainEvent.PropertyValues
            .Select(propertyChanged => $"{propertyChanged.PropertyName}: {propertyChanged.Value}")
            .ToList();

        var description = $"Created {entity.GetType().Name} {entity.Id}. Properties: {string.Join(", ", properties)}";
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
            Entity_Id = entity.Id,
            QualifiedTypeName = qualifiedTypeName,
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
