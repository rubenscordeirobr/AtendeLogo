using AtendeLogo.Domain.Entities.Identities;

namespace AtendeLogo.Domain.Extensions;

public static class UserSessionContextExtensions
{
    public static bool IsAnonymous(this IUserSession userSession)
    {
        Guard.NotNull(userSession);

        return userSession.User_Id == AnonymousIdentityConstants.User_Id ||
            userSession.AuthenticationType == AuthenticationType.Anonymous ||
            userSession.UserType == UserType.Anonymous;
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

        if (userSession.UserType  is UserType.AdminUser or UserType.SystemUser)
        {
            return userSession.UserRole is UserRole.Admin or UserRole.SystemAdmin;
        }
        return false;
            
    }
}
