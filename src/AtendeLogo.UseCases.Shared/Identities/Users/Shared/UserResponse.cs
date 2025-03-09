namespace AtendeLogo.UseCases.Identities.Users.Shared;

public abstract record UserResponse : IResponse, IUser
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string? ProfilePictureUrl { get; init; }
    public required PhoneNumber PhoneNumber { get; init; }
    public required Language Language { get; init; }
    public required UserRole Role { get; init; }
    public required UserState UserState { get; init; }
    public required UserStatus UserStatus { get; init; }
    public required VerificationState EmailVerificationState { get; init; }
    public required VerificationState PhoneNumberVerificationState { get; init; }
}
