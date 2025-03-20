namespace AtendeLogo.Domain.Entities.Identities;

public sealed class SystemUser : User
{
    // EF Core constructor
    private SystemUser(
       string name,
       string email,
       string? profilePictureUrl,
       Language language,
       UserRole role,
       UserState userState,
       UserStatus userStatus,
       VerificationState emailVerificationState,
       VerificationState phoneNumberVerificationState,
       PhoneNumber phoneNumber)
       : base(name, email, profilePictureUrl, language, role, userState, userStatus,
              emailVerificationState, phoneNumberVerificationState, phoneNumber, Password.Empty)
    {
    }

    public SystemUser(
        string name,
        string email,
        Language language,
        UserRole role,
        UserState userState,
        UserStatus userStatus,
        PhoneNumber phoneNumber,
        Password password)
        : base(name, email, null, language, role, userState, userStatus,
               VerificationState.NotVerified, VerificationState.NotVerified, phoneNumber, password)
    {
    }

    public override UserType UserType
        => Id == AnonymousIdentityConstants.User_Id
            ? UserType.Anonymous
            : UserType.SystemUser;
}
