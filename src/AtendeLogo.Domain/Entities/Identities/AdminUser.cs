﻿namespace AtendeLogo.Domain.Entities.Identities;

public sealed class AdminUser : User
{
    public override UserType UserType { get; } = UserType.AdminUser;

    // EF Core constructor
    private AdminUser(
        string name,
        string email,
        Culture culture,
        UserRole role,
        UserState userState,
        UserStatus userStatus,
        VerificationState emailVerificationState,
        VerificationState phoneNumberVerificationState,
        PhoneNumber phoneNumber)
        : base(name, email,  culture, role, userState, userStatus,
              emailVerificationState, phoneNumberVerificationState, phoneNumber, Password.Empty)
    {
    }

    public AdminUser(
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
    }
}
    
 
