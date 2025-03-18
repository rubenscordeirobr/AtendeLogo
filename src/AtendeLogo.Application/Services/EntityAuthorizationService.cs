using AtendeLogo.Domain.Enums;
using AtendeLogo.Domain.Extensions;
using AtendeLogo.Domain.Primitives.Contracts;

namespace AtendeLogo.Application.Services;

public class EntityAuthorizationService : IEntityAuthorizationService
{
    public void ValidateEntityChange(
         EntityBase entity,
         IUserSession userSession,
         EntityChangeState entityChangeState)
    {
        ValidateAuthorization(entity, userSession, entityChangeState);
        ValidateRole(entity, userSession, entityChangeState);
    }

    private void ValidateAuthorization(
        EntityBase entity,
        IUserSession userSession,
        EntityChangeState entityChangeState)
    {
        if (HasAuthorization(entity, userSession, entityChangeState))
        {
            return;
        }

        throw new UnauthorizedSecurityException(
           $"Access Denied: User {userSession.User_Id} (Role: {userSession.UserRole}, Tenant_Id: {userSession.Tenant_Id}) " +
           $"is not authorized to perform '{entityChangeState}' on entity '{entity.GetType().Name}' (ID: {entity.Id}, Tenant_Id: {(entity as IEntityTenant)?.Tenant_Id}).");
    }

    private bool HasAuthorization(
        EntityBase entity,
        IUserSession userSession,
        EntityChangeState entityChangeState)
    {
        if (userSession.IsAnonymous())
        {
            return CheckAnonymousPermission(entity, entityChangeState);
        }

        if (userSession.IsTenantUser())
        {
            if (entity is IEntityTenant entityTenant)
            {
                return entityTenant.Tenant_Id == userSession.Tenant_Id;
            }
            return false;
        }
        return userSession.UserRole is UserRole.SystemAdmin or UserRole.System;
    }

    private bool CheckAnonymousPermission(
        EntityBase entity,
        EntityChangeState entityChangeState)
    {
        return entityChangeState == EntityChangeState.Created && 
            entity is Tenant or TenantUser or UserSession;
    }

    private static void ValidateRole(
        EntityBase entity,
        IUserSession userSession,
        EntityChangeState state)
    {
        //not implement yet
    }
}
