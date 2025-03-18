using AtendeLogo.UseCases.Contracts.Identities;

namespace AtendeLogo.UseCases.Identities.Tenants.Services;

internal class TenantValidationService : ITenantValidationService
{
    private readonly ITenantRepository _tenantRepository;
    private readonly ITenantUserRepository _tenantUserRepository;

    public TenantValidationService(
        ITenantRepository tenantRepository,
        ITenantUserRepository tenantUserRepository)
    {
        _tenantRepository = tenantRepository;
        _tenantUserRepository = tenantUserRepository;
    }

    public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken token)
    {
        return await _tenantRepository.EmailExistsAsync(email, token) == false &&
            await _tenantUserRepository.EmailExistsAsync(email, token) == false;
    }

    public async Task<bool> IsEmailUniqueAsync(
        Guid currentTenant_Id,
        Guid currentTenantOwner_Id,
        string email,
        CancellationToken token)
    {
        return await _tenantRepository.EmailExistsAsync(currentTenant_Id, email, token) == false
            && await _tenantUserRepository.EmailExistsAsync(currentTenantOwner_Id, email, token) == false;
    }

    public async Task<bool> IsFiscalCodeUniqueAsync(
        string fiscalCode,
        CancellationToken token)
    {
        return await _tenantRepository
            .FiscalCodeExistsAsync(fiscalCode, token) == false;
    }

    public async Task<bool> IsFiscalCodeUniqueAsync(
        Guid currentTenant_Id,
        string fiscalCode,
        CancellationToken token)
    {
        return await _tenantRepository
            .FiscalCodeExistsAsync(currentTenant_Id, fiscalCode, token) == false;
    }

    public async Task<bool> IsPhoneNumberUniqueAsync(
        string phoneNumber,
        CancellationToken token)
    {
        return await _tenantRepository.PhoneNumberExitsAsync(phoneNumber, token) == false &&
            await _tenantUserRepository.PhoneNumberExitsAsync(phoneNumber, token) == false;
    }

    public async Task<bool> IsPhoneNumberUniqueAsync(
        Guid currentTenant_Id,
        Guid currentTenantOwner_Id,
        string phoneNumber,
        CancellationToken token)
    {
        return await _tenantRepository.PhoneNumberExitsAsync(currentTenant_Id, phoneNumber, token) == false
            && await _tenantUserRepository.PhoneNumberExitsAsync(currentTenantOwner_Id, phoneNumber, token) == false;
    }
}
