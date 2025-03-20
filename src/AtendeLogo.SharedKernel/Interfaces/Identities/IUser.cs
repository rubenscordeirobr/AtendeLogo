namespace AtendeLogo.Shared.Interfaces.Identities;

public interface IUser
{
    Guid Id { get; }
    string Name { get; }
    string Email { get; }
    string? ProfilePictureUrl { get; }
    Language Language { get; }
    UserRole Role { get; }
    UserType UserType { get; }
    VerificationState EmailVerificationState { get; }
    VerificationState PhoneNumberVerificationState { get; }
    PhoneNumber? PhoneNumber { get; }
}
