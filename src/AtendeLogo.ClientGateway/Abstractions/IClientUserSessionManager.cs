using AtendeLogo.Shared.Models.Security;

namespace AtendeLogo.ClientGateway.Abstractions;

public interface IClientAuthorizationTokenManager
{
    UserSessionClaims? GetUserSessionClaims();
    UserSessionState? GetUserSessionState();

    Task SetAuthorizationTokenAsync(string authorizationToken, bool isPersistent);
    Task ValidateAuthorizationHeaderAsync(string responseAuthorizationHeader);
    Task<string?> GetAuthorizationTokenAsync();
    Task EnsureUserSessionStateAsync();
    Task RemoveAuthorizationTokenAsync();

    event EventHandlerAsync<AuthorizationTokenUpdatedEventArgs>? UserSessionStateUpdated;
}
