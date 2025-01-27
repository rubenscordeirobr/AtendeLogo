
namespace AtendeLogo.Domain.Entities.Identities;

public class AdminUser : User
{
    public AdminUserRole AdminUserRole { get; private set; }
    public AdminUser(
        string name,
        string email,
        string phoneNumber,
        Password password,
        UserState userState,
        UserStatus userStatus) :
        base(name, email, phoneNumber, password, userState, userStatus)
    {
    }
}
