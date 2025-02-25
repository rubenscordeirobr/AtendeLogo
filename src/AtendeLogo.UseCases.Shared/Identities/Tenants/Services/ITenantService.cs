using AtendeLogo.Common;
using AtendeLogo.UseCases.Identities.Tenants.Commands;

namespace AtendeLogo.UseCases.Identities.Tenants.Services;

public interface ITenantService
{
    Task<Result<CreateTenantResponse>> CreateTenantAsync(
        CreateTenantCommand command,
        CancellationToken cancellationToken = default);
}

