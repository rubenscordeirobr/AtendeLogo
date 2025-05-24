using System.Security.Claims;
using AtendeLogo.ClientGateway.Abstractions;
using Microsoft.AspNetCore.Components.Authorization;

namespace AtendeLogo.TenantPortal.Services;

public abstract class TenantUserAuthenticationStateProvider : AuthenticationStateProvider
{
    protected ILogger Logger { get; }
    protected IClientAuthorizationTokenManager TokenManager { get; }
    protected IClientTenantUserSessionContextService SessionContextService { get; }
    private readonly AuthenticationState _anonymous = new(new(new ClaimsIdentity()));

    protected TenantUserAuthenticationStateProvider(
        IClientAuthorizationTokenManager tokenManager,
        ILogger logger)
    {
        Guard.NotNull(tokenManager);

        TokenManager = tokenManager;
        Logger = logger;

        tokenManager.UserSessionStateUpdated += OnUserSessionStateAsync;
    }
     
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            await TokenManager.EnsureUserSessionStateAsync();

            var userSessionsClaims = TokenManager.GetUserSessionClaims();
            if (userSessionsClaims is null || userSessionsClaims.IsAnonymous())
            {
                return _anonymous;
            }
             
            var principal = userSessionsClaims.ToClaimsPrincipal();
            return new AuthenticationState(principal);
             
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "GetAuthenticationStateAsync");
            return _anonymous;
        }
    }

    protected virtual Task OnUserSessionStateAsync(object? sender, AuthorizationTokenUpdatedEventArgs e)
    {
        return Task.CompletedTask;
    }
}

