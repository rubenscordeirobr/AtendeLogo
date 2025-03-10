﻿using AtendeLogo.Domain.Entities.Identities;

namespace AtendeLogo.Domain.Extensions;

public static class UserSessionContextExtensions
{
    public static bool IsAnonymous(this IUserSession userSession)
    {
        Guard.NotNull(userSession);

        return userSession.User_Id == AnonymousIdentityConstants.AnonymousUser_Id ||
            userSession.AuthenticationType == AuthenticationType.Anonymous;
    }

    public static bool IsTenantUser(this IUserSession userSession)
    {
        Guard.NotNull(userSession);

        return userSession.Tenant_Id.HasValue
            && userSession.Tenant_Id != Guid.Empty;
    }

    public static bool IsSystemAdminUser(this IUserSession userSession)
    {
        Guard.NotNull(userSession);
        if (userSession.IsAnonymous())
        {
            return false;
        }

        Guard.NotNull(userSession.User);
        return userSession.User is AdminUser;
    }
}
