using AtendeLogo.UseCases.Identities.Authentications.Commands;

namespace AtendeLogo.UseCases.Contracts.Identities;

public interface ITenantUserAuthenticationService : ICommunicationService
{
    Task<Result<TenantUserLoginResponse>> LoginAsync(
        TenantUserLoginCommand command,
        CancellationToken cancellationToken = default);


    Task<Result<OperationResponse>> LogoutAsync(
        TenantUserLogoutCommand command,
        CancellationToken cancellationToken = default);
}
