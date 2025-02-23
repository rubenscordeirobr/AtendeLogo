namespace AtendeLogo.Domain.Entities.Identities;

public sealed class SystemUser : User
{
    // EF Core constructor
    private SystemUser(
       string name,
       string email,
       UserState userState,
       UserStatus userStatus,
       PhoneNumber phoneNumber) :
       this(name, email, userState, userStatus, phoneNumber, Password.Empty)
    {

    }

    public SystemUser(
        string name, 
        string email, 
        UserState userState, 
        UserStatus userStatus, 
        PhoneNumber phoneNumber,
        Password password) 
        : base(name, email, userState, userStatus, phoneNumber, password)
    {
    }
}
