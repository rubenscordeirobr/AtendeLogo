namespace AtendeLogo.Domain.Entities.Identities;

public sealed class AdminUser : User
{
    public AdminUserRole AdminUserRole { get; private set; }
     
    private AdminUser(
        string name,
        string email,
        UserState userState,
        UserStatus userStatus,
        AdminUserRole adminUserRole,
        PhoneNumber phoneNumber)  :
        this(name, email, userState, userStatus, adminUserRole, phoneNumber, Password.Empty)
    {
        
    }
    public AdminUser(
        string name,
        string email,
        UserState userState,
        UserStatus userStatus,
        AdminUserRole adminUserRole,
        PhoneNumber phoneNumber,
        Password password) : base(name, email, userState, userStatus, phoneNumber, password)
    {
        AdminUserRole = adminUserRole;
    }
}
