namespace AtendeLogo.Domain.Entities.Identities;

public sealed class TenantUser : User, ITenantOwned, ISoftDeletableEntity
{
    public Guid Tenant_Id { get; private set; }
    public Tenant? Tenant { get; private set; }
    public TenantUserRole TenantUserRole { get; private set; }
     
    // EF Core constructor
    private TenantUser(
       string name,
       string email,
       UserState userState,
       UserStatus userStatus,
       TenantUserRole tenantUserRole,
       PhoneNumber phoneNumber) :
       base(name, email, userState, userStatus,  phoneNumber, Password.Empty)
    {
        TenantUserRole = tenantUserRole;
    }

    public TenantUser(
        Tenant tenant,
        string name,
        string email,
        UserState userState,
        UserStatus userStatus,
        TenantUserRole tenantUserRole,
        PhoneNumber phoneNumber,
        Password password)
        : base(name, email, userState, userStatus, phoneNumber, password)
    {
        Guard.NotNull(tenant, nameof(tenant));

        Tenant = tenant;
        TenantUserRole = tenantUserRole;
    }
}
