namespace AtendeLogo.Domain.Entities.Identities;

public sealed class TenantUser : User, ITenantOwned
{
    public override UserType UserType { get; } = UserType.TenantUser;

    public Guid Tenant_Id { get; private set; }
    public Tenant? Tenant { get; private set; }
     
    // EF Core constructor
    private TenantUser(
       string name,
       string email,
       Culture culture,
       UserRole role,
       UserState userState,
       UserStatus userStatus,
       VerificationState emailVerificationState,
       VerificationState phoneNumberVerificationState,
       PhoneNumber phoneNumber)
       : base(name, email, culture, role, userState, userStatus,
              emailVerificationState, phoneNumberVerificationState, phoneNumber, Password.Empty)
    {
    }

    public TenantUser(
        Tenant tenant,
        string name,
        string email,
        Culture culture,
        UserRole role,
        UserState userState,
        UserStatus userStatus,
        PhoneNumber phoneNumber,
        Password password)
        : base(name, email, culture, role, userState, userStatus, 
               VerificationState.NotVerified, VerificationState.NotVerified, phoneNumber, password)
    {
        Guard.NotNull(tenant);
        Tenant = tenant;
    }
}
