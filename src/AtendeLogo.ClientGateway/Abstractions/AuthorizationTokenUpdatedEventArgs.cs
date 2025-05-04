using AtendeLogo.Shared.Models.Security;

namespace AtendeLogo.ClientGateway.Abstractions;

public class AuthorizationTokenUpdatedEventArgs : EventArgs
{
    public UserSessionState? UserSessionState { get; }

    public AuthorizationTokenUpdatedEventArgs(UserSessionState? userSessionState )
    {
        UserSessionState = userSessionState;
    }
}

public record UserSessionState(
    UserSessionClaims? UserSessionClaims,
    string? AuthorizationToken);
