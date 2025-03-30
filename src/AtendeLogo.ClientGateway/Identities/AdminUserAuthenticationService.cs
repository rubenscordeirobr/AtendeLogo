using AtendeLogo.Shared.Enums;
using AtendeLogo.UseCases.Identities.Authentications.Commands;

namespace AtendeLogo.ClientGateway.Identities;

[Route(IdentityRouteConstants.AdminUserAuthentication)]
public class AdminUserAuthenticationService : IAdminUserAuthenticationService
{
    private readonly IClientAuthorizationTokenManager _tokenAuthorizationTokenManager;
    private readonly IClientAdminUserSessionContext _clientUserSessionContext;
    private readonly IHttpClientMediator<AdminUserAuthenticationService> _mediator;

    public AdminUserAuthenticationService(
        IClientAuthorizationTokenManager clientAuthorizationTokenManager,
        IClientAdminUserSessionContext clientUserSessionContext,
        IHttpClientMediator<AdminUserAuthenticationService> mediator)
    {
        _mediator = mediator;
        _tokenAuthorizationTokenManager = clientAuthorizationTokenManager;
        _clientUserSessionContext = clientUserSessionContext;
    }

    public async Task<Result<AdminUserLoginResponse>> LoginAsync(
        AdminUserLoginCommand command,
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
                return Result.Failure<AdminUserLoginResponse>(
                    new AuthenticationError(
                        "IClientAuthorizationTokenManager.UserSessionClaimsInvalid",
                        "Failed to set authorization token"));
            }

            if (claims.UserType != UserType.AdminUser)
            {
                return Result.Failure<AdminUserLoginResponse>(
                    new AuthenticationError(
                        "IClientAuthorizationTokenManager.UserTypeInvalid",
                        "User type is invalid"));
            }

            _clientUserSessionContext.SetSessionContext(
                claims,
                response.UserSession,
                response.User);
        }
        return result;
    }

    public async Task<Result<OperationResponse>> LogoutAsync(
        AdminUserLogoutCommand command,
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

