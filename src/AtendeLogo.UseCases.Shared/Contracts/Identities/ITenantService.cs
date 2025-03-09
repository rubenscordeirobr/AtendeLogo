namespace AtendeLogo.UseCases.Contracts.Identities;

public interface ITenantService : IEndpointService
{
    Task<Result<CreateTenantResponse>> CreateTenantAsync(
        CreateTenantCommand command,
        CancellationToken cancellationToken = default);
}
