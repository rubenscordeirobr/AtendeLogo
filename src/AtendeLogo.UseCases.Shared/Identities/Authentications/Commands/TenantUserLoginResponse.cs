namespace AtendeLogo.UseCases.Identities.Authentications.Commands;

public record TenantUserLoginResponse : IResponse
{
    public required Guid User_Id { get; init; }
    public required Guid Tenant_Id { get; init; }
    public required string ClientSessionToken { get; init; }
    public required string Email { get; init; }
    public required string PhoneNumber { get; init; }
    public required string FullName { get; init; }
    public required string? ProfilePictureUrl { get; init; }
    public required bool IsEmailVerified { get; init; }
    public required bool IsPhoneNumberVerified { get; init; }
    public required UserRole Role { get; init; }

}

