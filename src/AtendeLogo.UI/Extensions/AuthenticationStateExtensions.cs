using Microsoft.AspNetCore.Components.Authorization;

namespace AtendeLogo.UI.Extensions;

public static class AuthenticationStateExtensions
{
    public static bool IsAuthenticated(this AuthenticationState authenticationState)
    {
        return authenticationState?.User.Identity?.IsAuthenticated ?? false;
    }
}
