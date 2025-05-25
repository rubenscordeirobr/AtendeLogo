using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;

namespace AtendeLogo.TenantPortal.BlazorServer.Services;

internal sealed class HybridTenantUserAuthenticationStateProvider : TenantUserAuthenticationStateProvider
{
    private readonly HttpContextSignInHandler _httpContextoSingInHandler;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HybridTenantUserAuthenticationStateProvider(
        HttpContextSignInHandler httpContextoSingInHandler,
        IClientAuthorizationTokenManager tokenManager,
        IHttpContextAccessor httpContextAccessor,
        ILogger<HybridTenantUserAuthenticationStateProvider> logger)
        : base(tokenManager, logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _httpContextoSingInHandler = httpContextoSingInHandler;
    }

    protected override async Task OnUserSessionStateAsync(object? sender, AuthorizationTokenUpdatedEventArgs e)
    {
        await base.OnUserSessionStateAsync(sender, e);
        await EnsureSignInBlazorServerAsync(e.UserSessionState);
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var state = await base.GetAuthenticationStateAsync();
        if (state.IsAuthenticated())
        {
            await EnsureSignInBlazorServerAsync();
            return state;
        }

        var stateFromHttpContext = await GetAuthenticationStateFromHttpContextAsync();
        if (stateFromHttpContext is not null && stateFromHttpContext.IsAuthenticated())
        {
            return stateFromHttpContext;
        }
        return state;
    }

    private async Task<AuthenticationState?> GetAuthenticationStateFromHttpContextAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is null)
            return null;

        var authResult = await httpContext.AuthenticateAsync(TenantUserAuthenticationConfig.AuthenticationScheme);
        if (!authResult.Succeeded)
        {
            return null;
        }

        var principal = authResult.Principal;
        if (principal is null)
        {
            return null;
        }
        return new AuthenticationState(principal);
    }

    private async Task EnsureSignInBlazorServerAsync()
    {
        var userSessionState = TokenManager.GetUserSessionState();
        await EnsureSignInBlazorServerAsync(userSessionState);
    }

    private async Task EnsureSignInBlazorServerAsync(UserSessionState? userSessionState)
    {
        try
        {
            await _httpContextoSingInHandler.HandleUserAuthenticationAsync(userSessionState);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, " Error signing {ErrorMessage}", ex.GetNestedMessage());
        }
    }
}

