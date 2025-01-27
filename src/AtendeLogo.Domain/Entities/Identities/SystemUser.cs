
namespace AtendeLogo.Domain.Entities.Identities;

public class SystemUser : User
{
    public SystemUser(
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
