using AtendeLogo.Shared.Enums;
using AtendeLogo.UseCases.Identities.Authentications.Commands;

namespace AtendeLogo.ClientGateway.Identities;

[Route(IdentityRouteConstants.TenantUserAuthentication)]
public class TenantUserAuthenticationService : ITenantUserAuthenticationService
{
    private readonly IClientAuthorizationTokenManager _tokenAuthorizationTokenManager;
    private readonly IClientTenantUserSessionContext _clientUserSessionContext;
    private readonly IHttpClientMediator<TenantUserAuthenticationService> _mediator;

    public TenantUserAuthenticationService(
        IClientAuthorizationTokenManager clientAuthorizationTokenManager,
        IClientTenantUserSessionContext clientUserSessionContext,
        IHttpClientMediator<TenantUserAuthenticationService> mediator)
    {
        _mediator = mediator;
        _tokenAuthorizationTokenManager = clientAuthorizationTokenManager;
        _clientUserSessionContext = clientUserSessionContext;
    }

    public async Task<Result<TenantUserLoginResponse>> LoginAsync(
        TenantUserLoginCommand command,
        CancellationToken cancellationToken = default)
    {
        Guard.NotNull(command);

        var result = await _mediator.PostAsync(command, cancellationToken);
        if (result.IsSuccess)
        {
            var response = result.Value;

            await _tokenAuthorizationTokenManager.SetAuthorizationTokenAsync(
                response.AuthorizationToken,
                command.KeepSession);

            var claims = _tokenAuthorizationTokenManager.GetUserSessionClaims();
            if (claims is null)
            {
                return Result.Failure<TenantUserLoginResponse>(
                    new AuthenticationError(
                        "IClientAuthorizationTokenManager.UserSessionClaimsInvalid",
                        "Failed to set authorization token"));
            }

            if(claims.UserType != UserType.TenantUser)
            {
                return Result.Failure<TenantUserLoginResponse>(
                    new AuthenticationError(
                        "IClientAuthorizationTokenManager.UserTypeInvalid",
                        "User type is invalid"));
            }
             
            _clientUserSessionContext.SetSessionContext(
                claims,
                response.UserSession,
                response.User,
                response.Tenant);
        }
        return result;
    }

    public async Task<Result<OperationResponse>> LogoutAsync(
        TenantUserLogoutCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.PostAsync(command,
            IdentityRouteConstants.Logout,
            cancellationToken);

        if (result.IsSuccess)
        {
            await _tokenAuthorizationTokenManager.RemoveAuthorizationTokenAsync();
            _clientUserSessionContext.ClearSessionContext();
        }
        return result;
    }
}

