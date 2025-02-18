using AtendeLogo.Application.Contracts.Persistence.Identity;

namespace AtendeLogo.UseCases.Identities.Tenants.Services;

internal class TenantValidationService : ITenantValidationService
{
    private readonly ITenantRepository _tenantRepository;

    public TenantValidationService(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken token)
    {
        return await _tenantRepository.EmailExistsAsync(email, token) == false;
    }

    public async Task<bool> IsEmailUniqueAsync(Guid currentTenant_Id, string email, CancellationToken token)
    {
        return await _tenantRepository.EmailExistsAsync(currentTenant_Id, email, token) == false;
    }

    public async Task<bool> IsFiscalCodeUniqueAsync(string fiscalCode, CancellationToken token)
    {
        return await _tenantRepository
            .FiscalCodeExistsAsync(fiscalCode, token) == false;
    }

    public async Task<bool> IsFiscalCodeUniqueAsync(Guid currentTenant_Id, string fiscalCode, CancellationToken token)
    {
        return await _tenantRepository
            .FiscalCodeExistsAsync(currentTenant_Id, fiscalCode, token) == false;
    }
}
