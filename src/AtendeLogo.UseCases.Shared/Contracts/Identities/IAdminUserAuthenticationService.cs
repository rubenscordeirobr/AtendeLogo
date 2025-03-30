using AtendeLogo.UseCases.Identities.Authentications.Commands;

namespace AtendeLogo.UseCases.Contracts.Identities;

public interface IAdminUserAuthenticationService : ICommunicationService
{
    Task<Result<AdminUserLoginResponse>> LoginAsync(
        AdminUserLoginCommand command,
        CancellationToken cancellationToken = default);

    Task<Result<OperationResponse>> LogoutAsync(
        AdminUserLogoutCommand command,
        CancellationToken cancellationToken = default);
}
