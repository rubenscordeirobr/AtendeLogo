namespace AtendeLogo.UseCases.Identities.Authentications.Commands;

public record TenantUserLogoutCommand: CommandRequest<TenantUserLogoutResponse>
{
    public required string ClientSessionToken { get; init; }

}
