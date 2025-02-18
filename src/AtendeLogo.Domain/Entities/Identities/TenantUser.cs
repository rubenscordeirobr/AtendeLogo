namespace AtendeLogo.Domain.Entities.Identities;

public sealed class TenantUser : User, ITenantOwned, ISoftDeletableEntity
{
    public Guid Tenant_Id { get; private set; }
    public Tenant? Tenant { get; private set; }
    public TenantUserRole TenantUserRole { get; private set; }
    public List<TenantUser> TenantUsers { get; } = [];

    private TenantUser(
       string name,
       string email,
       UserState userState,
       UserStatus userStatus,
       TenantUserRole tenantUserRole,
       PhoneNumber phoneNumber) :
       this(name, email, userState, userStatus, tenantUserRole, phoneNumber, Password.Empty)
    {
        TenantUserRole = tenantUserRole;
    }

    public TenantUser(
        string name,
        string email,
        UserState userState,
        UserStatus userStatus,
        TenantUserRole tenantUserRole,
        PhoneNumber phoneNumber,
        Password password)
        : base(name, email, userState, userStatus, phoneNumber, password)
    {
    }

    public void SetTenant(Tenant tenant)
    {
        if (Id != default)
            throw new InvalidOperationException("Tenant can be set only for new user");
        
        if (tenant.Id == default)
            throw new InvalidOperationException("Tenant must have an Id");

        if (Tenant_Id != default)
            throw new InvalidOperationException("Tenant can be set only once");

        Tenant = tenant;
        Tenant_Id = tenant.Id;
    }
}
