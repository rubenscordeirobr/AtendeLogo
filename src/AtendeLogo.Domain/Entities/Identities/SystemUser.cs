namespace AtendeLogo.Domain.Entities.Identities;

public sealed class SystemUser : User
{
    public SystemUser(
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

    public void SetAnonymousId()
    {
        if (Id != default)
        {
            throw new InvalidOperationException("Id is already set.");
        }
        SetCreateSession(AnonymousConstants.AnonymousSystemSession_Id);
        Id = AnonymousConstants.AnonymousUser_Id;
        Email = AnonymousConstants.AnonymousEmail;
        Name = AnonymousConstants.AnonymousName;
        UserStatus = UserStatus.Anonymous;
        UserState = UserState.Active;
    }
}
