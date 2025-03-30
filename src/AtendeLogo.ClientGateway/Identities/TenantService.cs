using AtendeLogo.UseCases.Identities.Tenants.Commands;
using AtendeLogo.UseCases.Identities.Tenants.Queries;

namespace AtendeLogo.ClientGateway.Identities;

[Route(IdentityRouteConstants.Tenants)]
public class TenantService : ITenantService
{
    private readonly IHttpClientMediator<TenantService> _mediator;

    public TenantService(IHttpClientMediator<TenantService> mediator)
    {
        _mediator = mediator;
    }

    #region Queries

    public Task<Result<TenantResponse>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetTenantByIdQuery(id);
        return _mediator.GetAsync(
            query,
            RouteConstants.RouteId,
            cancellationToken);
    }

    #endregion

    #region Commands

    public Task<Result<CreateTenantResponse>> CreateAsync(
        CreateTenantCommand command,
        CancellationToken cancellationToken = default)
    {
        return _mediator.PostAsync(
            command,
            cancellationToken);
    }

    public Task<Result<OperationResponse>> UpdateAsync(
        UpdateTenantCommand command,
        CancellationToken cancellationToken = default)
    {
        return _mediator.PutAsync(
            command,
            cancellationToken);
    }

    public Task<Result<OperationResponse>> UpdateDefaultAddressAsync(
        UpdateDefaultTenantAddressCommand command,
        CancellationToken cancellationToken = default)
    {
        return _mediator.PutAsync(
            command,
            IdentityRouteConstants.TenantUpdateDefaultAddress,
            cancellationToken);
    }

    public Task<Result<OperationResponse>> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteTenantCommand(id);
        return _mediator.DeleteAsync(
            command,
            cancellationToken);
    }

    public Task<Result<OperationResponse>> DeleteAsync(
        DeleteTenantCommand command, 
        CancellationToken cancellationToken = default)
    {
        return _mediator.DeleteAsync(
           command,
           cancellationToken);
    }

    #endregion
}
