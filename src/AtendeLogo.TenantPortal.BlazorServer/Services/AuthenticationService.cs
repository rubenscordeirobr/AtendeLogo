using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace AtendeLogo.TenantPortal.BlazorServer.Services;

internal sealed class HttpContextTenantUserAuthenticationService : AuthenticationService, IAuthenticationService
{
    public HttpContextTenantUserAuthenticationService(
        IAuthenticationSchemeProvider schemes,
        IAuthenticationHandlerProvider handlers, 
        IClaimsTransformation transform,
        IOptions<AuthenticationOptions> options) 
        : base(schemes, handlers, transform, options)
    {
    }

    public override Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string? scheme)
    {
        return base.AuthenticateAsync(context, scheme);
    }

    public override Task SignInAsync(HttpContext context, string? scheme, ClaimsPrincipal principal, AuthenticationProperties? properties)
    {
        return base.SignInAsync(context, scheme, principal, properties);
    }

    public override Task SignOutAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
    {
        return base.SignOutAsync(context, scheme, properties);
    }

    public override Task ForbidAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
    {
        return base.ForbidAsync(context, scheme, properties);
    }

    public override Task ChallengeAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
    {
        return base.ChallengeAsync(context, scheme, properties);
    }

}

