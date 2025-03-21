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

    public async Task<bool> IsEmailUniqueAsync(
        string email, 
        CancellationToken cancellationToken = default)
    {
        return !await _tenantRepository.EmailExistsAsync(email, cancellationToken) &&
!await _tenantUserRepository.EmailExistsAsync(email, cancellationToken);
    }

    public async Task<bool> IsEmailUniqueAsync(
        Guid currentTenant_Id,
        Guid currentTenantOwner_Id,
        string email,
        CancellationToken cancellationToken = default)
    {
        return !await _tenantRepository.EmailExistsAsync(currentTenant_Id, email, cancellationToken)
            && !await _tenantUserRepository.EmailExistsAsync(currentTenantOwner_Id, email, cancellationToken);
    }

    public async Task<bool> IsFiscalCodeUniqueAsync(
        string fiscalCode,
        CancellationToken cancellationToken = default)
    {
        return !await _tenantRepository
            .FiscalCodeExistsAsync(fiscalCode, cancellationToken);
    }

    public async Task<bool> IsFiscalCodeUniqueAsync(
        Guid currentTenant_Id,
        string fiscalCode,
        CancellationToken cancellationToken = default)
    {
        return !await _tenantRepository
            .FiscalCodeExistsAsync(currentTenant_Id, fiscalCode, cancellationToken);
    }

    public async Task<bool> IsPhoneNumberUniqueAsync(
        string phoneNumber,
        CancellationToken cancellationToken = default)
    {
        return !await _tenantRepository.PhoneNumberExitsAsync(phoneNumber, cancellationToken) &&
            !await _tenantUserRepository.PhoneNumberExitsAsync(phoneNumber, cancellationToken);
    }

    public async Task<bool> IsPhoneNumberUniqueAsync(
        Guid currentTenant_Id,
        Guid currentTenantOwner_Id,
        string phoneNumber,
        CancellationToken cancellationToken = default)
    {
        return !await _tenantRepository.PhoneNumberExitsAsync(currentTenant_Id, phoneNumber, cancellationToken)
            && !await _tenantUserRepository.PhoneNumberExitsAsync(currentTenantOwner_Id, phoneNumber, cancellationToken);
    }
}
