namespace AtendeLogo.UseCases.Identities.Users.Shared;

public abstract record UserResponse : ResponseBase, IUser
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required PhoneNumber PhoneNumber { get; init; }
    public required UserState UserState { get; init; }
    public required UserStatus UserStatus { get; init; }
}
