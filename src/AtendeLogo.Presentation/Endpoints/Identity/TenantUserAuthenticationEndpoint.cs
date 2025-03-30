using AtendeLogo.UseCases.Identities.Authentications.Commands;
using Microsoft.AspNetCore.Authorization;

namespace AtendeLogo.Presentation.Endpoints.Identity;

[ServiceRole(ServiceRole.Authentication)]
[EndPoint(IdentityRouteConstants.TenantUserAuthentication)]
public class TenantUserAuthenticationEndpoint : ApiEndpointBase, ITenantUserAuthenticationService
{
    private readonly IRequestMediator _mediator;
 
    public TenantUserAuthenticationEndpoint(IRequestMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [AllowAnonymous]
    public Task<Result<TenantUserLoginResponse>> LoginAsync(
        TenantUserLoginCommand command,
        CancellationToken cancellationToken = default)
    {
        return _mediator.RunAsync(command, cancellationToken);
    }

    [HttpPost(IdentityRouteConstants.Logout)]
    public Task<Result<OperationResponse>> LogoutAsync(
        TenantUserLogoutCommand command,
        CancellationToken cancellationToken = default)
    {
        return _mediator.RunAsync(command, cancellationToken);
    }

}

