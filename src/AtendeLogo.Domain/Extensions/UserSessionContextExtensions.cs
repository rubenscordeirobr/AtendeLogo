namespace AtendeLogo.Domain.Extensions;

public static class UserSessionContextExtensions
{
    public static bool IsAnonymous(this IUserSession userSession)
    {
        ArgumentNullException.ThrowIfNull(userSession);

        return userSession.User_Id == AnonymousConstants.AnonymousUser_Id ||
            userSession.AuthenticationType == AuthenticationType.Anonymous;
    }

    public static bool IsTenantUser(this IUserSession userSession)
    {
        ArgumentNullException.ThrowIfNull(userSession);

        return userSession.Tenant_Id.HasValue
            && userSession.Tenant_Id != Guid.Empty;
    }
}
