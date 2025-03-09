using AtendeLogo.UseCases.Identities.Authentications.Commands;

namespace AtendeLogo.UseCases.Contracts.Identities;

public interface ITenantAuthenticationService : IEndpointService
{
    Task<Result<TenantUserLoginResponse>> LoginAsync(TenantUserLoginCommand command);

    Task<Result<TenantUserLogoutResponse>> LogoutAsync(TenantUserLogoutCommand command);
}
