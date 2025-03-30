namespace AtendeLogo.UseCases.Identities.Authentications.Commands;

public record TenantUserLogoutCommand(
    Guid Session_Id)
    : CommandRequest<OperationResponse>;

