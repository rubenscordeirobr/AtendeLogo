using AtendeLogo.Shared.Models.Security;

namespace AtendeLogo.ClientGateway.Common.Abstractions;

public interface IClientAuthorizationTokenManager
{
    UserSessionClaims? GetUserSessionClaims();
    Task SetAuthorizationTokenAsync(string authorizationToken, bool keepSession);
    Task ValidateAuthorizationHeaderAsync(string responseAuthorizationHeader);
    Task<string?> GetAuthorizationTokenAsync();
    Task RemoveAuthorizationTokenAsync();
}
