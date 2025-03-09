using AtendeLogo.Shared.Contantes;
using AtendeLogo.Shared.Interfaces.Identities;

namespace AtendeLogo.Shared.Models.Identities;

public class AnonymousUser : IUser
{
    public Guid Id { get; } = AnonymousIdentityConstants.AnonymousUser_Id;

    public string Name { get; } = AnonymousIdentityConstants.AnonymousName;

    public string Email { get; } = AnonymousIdentityConstants.AnonymousEmail;

    public VerificationState EmailVerificationState { get; } = VerificationState.Verified;

    public VerificationState PhoneNumberVerificationState { get; } = VerificationState.Verified;

    public string? ProfilePictureUrl { get; } = null;

    public PhoneNumber? PhoneNumber { get; } = null;

    public Language Language { get; } = Language.Default;

    public UserRole Role { get; } = UserRole.None;
}
