namespace AtendeLogo.Domain.Extensions;

public static class EntityDeletedExtensions
{
    public static void MarkAsDeleted(
        this ISoftDeletableEntity entity,
        IUserSession userSession)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(userSession);

        if (userSession.IsAnonymous())
        {
            throw new InvalidOperationException("Cannot delete entity with anonymous session");
        }

        if (userSession.IsTenantUser() && entity is IEntityTenant entityTenant)
        {
            if (entityTenant.Tenant_Id != userSession.Tenant_Id)
            {
                throw new InvalidOperationException("Cannot delete entity from another tenant");
            }
        }

        var entityType = entity.GetType();
        var properties = entityType.GetPropertiesFromInterface<ISoftDeletableEntity>();

        properties[nameof(ISoftDeletableEntity.DeletedAt)]
            .SetValue(entity, DateTime.UtcNow);

        properties[nameof(ISoftDeletableEntity.DeletedSession_Id)]
            .SetValue(entity, userSession.Id);

        properties[nameof(ISoftDeletableEntity.IsDeleted)]
            .SetValue(entity, true);

    }
}
