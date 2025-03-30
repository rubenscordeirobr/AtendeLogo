namespace AtendeLogo.UseCases.Identities.Authentications.Commands;

public record AdminUserLoginCommand : CommandRequest<AdminUserLoginResponse>
{
    public required string EmailOrPhoneNumber { get; init; }
    public required string Password { get; init; }
    public required bool KeepSession { get; init; }
}


