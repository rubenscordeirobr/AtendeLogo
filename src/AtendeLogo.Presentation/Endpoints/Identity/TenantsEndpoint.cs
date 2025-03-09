using AtendeLogo.Application.Contracts.Mediators;
using AtendeLogo.Presentation.Common;
using AtendeLogo.UseCases.Contracts.Identities;
using AtendeLogo.UseCases.Identities.Tenants.Commands;

namespace AtendeLogo.Presentation.Endpoints.Identity;

[EndPoint("api/tenants")]
public class TenantsEndpoint : ApiEndpointBase, ITenantService
{

    private readonly IRequestMediator _mediator;

    public TenantsEndpoint(IRequestMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public Task<Result<CreateTenantResponse>> CreateTenantAsync(
        CreateTenantCommand command,
        CancellationToken cancellationToken = default)
    {
        return _mediator.RunAsync(command, cancellationToken);
    }
}

