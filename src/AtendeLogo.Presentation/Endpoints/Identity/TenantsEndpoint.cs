﻿using AtendeLogo.UseCases.Identities.Tenants.Commands;
using AtendeLogo.UseCases.Identities.Tenants.Queries;
using Microsoft.AspNetCore.Authorization;

namespace AtendeLogo.Presentation.Endpoints.Identity;

[EndPoint(IdentityRouteConstants.Tenants)]
public class TenantsEndpoint : ApiEndpointBase, ITenantService
{

    private readonly IRequestMediator _mediator;

    public TenantsEndpoint(IRequestMediator mediator)
    {
        _mediator = mediator;
    }
     
    [HttpGet(RouteConstants.RouteId)]
    public Task<Result<TenantResponse>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetTenantByIdQuery(id);
        return _mediator.GetAsync(query, cancellationToken);
    }

    [HttpPost]
    [AllowAnonymous]
    public Task<Result<CreateTenantResponse>> CreateAsync(
        CreateTenantCommand command,
        CancellationToken cancellationToken = default)
    {
        return _mediator.RunAsync(command, cancellationToken);
    }

  

    [HttpPut]
    public Task<Result<OperationResponse>> UpdateAsync(
        UpdateTenantCommand command,
        CancellationToken cancellationToken = default)
    {
        return _mediator.RunAsync(command, cancellationToken);
    }

    [HttpDelete]
    public Task<Result<OperationResponse>> DeleteAsync(
        DeleteTenantCommand command, 
        CancellationToken cancellationToken = default)
    {
        return _mediator.RunAsync(command, cancellationToken);
    }
}
