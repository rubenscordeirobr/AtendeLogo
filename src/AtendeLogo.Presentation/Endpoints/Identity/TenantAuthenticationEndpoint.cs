using AtendeLogo.Application.Contracts.Mediators;
using AtendeLogo.Presentation.Common;
using AtendeLogo.Shared.Enums;
using AtendeLogo.UseCases.Contracts.Identities;
using AtendeLogo.UseCases.Identities.Authentications.Commands;

namespace AtendeLogo.Presentation.Endpoints.Identity;

[EndPoint("api/identity/tenant-authentication-validation")]
public class TenantAuthenticationEndpoint : ApiEndpointBase, ITenantAuthenticationService
{
    private readonly IRequestMediator _mediator;

    public TenantAuthenticationEndpoint(IRequestMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public Task<Result<TenantUserLoginResponse>> LoginAsync(
        TenantUserLoginCommand command)
    {
        return _mediator.RunAsync(command);
    }

    [HttpPost]
    public Task<Result<TenantUserLogoutResponse>> LogoutAsync(TenantUserLogoutCommand command)
    {
        return _mediator.RunAsync(command);
    }

    #region IEndpointService

    public override bool IsAllowAnonymous
        => true;

    public override ServiceRole ServiceRole
        => ServiceRole.UserAuthentication;
    #endregion
}

