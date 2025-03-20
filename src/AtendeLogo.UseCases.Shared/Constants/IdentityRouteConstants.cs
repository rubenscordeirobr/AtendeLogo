namespace AtendeLogo.UseCases.Constants;

public static class IdentityRouteConstants
{
    private const string IdentityBase = $"{RouteConstants.ApiBase}/identity";
     
    public const string Tenants = $"{IdentityBase}/tenants";
    public const string TenantValidation = $"{IdentityBase}/tenant-validation";
    public const string TenantUserValidation = $"{IdentityBase}/tenant-users-validation";
    public const string TenantAuthentication = $"{IdentityBase}/tenant-authentication";
    public const string TenantAuthenticationValidation = $"{IdentityBase}/tenant-authentication-validation";
    public const string TenantUpdateDefaultAddress = "update-default-address";

    public const string TenantUsersEndpoint = $"{IdentityBase}/tenant-users";
    public const string AdminUsersRoute = $"{IdentityBase}/admin-users";

    public const string Logout = "logout";
}
