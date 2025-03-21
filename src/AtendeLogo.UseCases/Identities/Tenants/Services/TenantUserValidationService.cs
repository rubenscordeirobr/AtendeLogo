using AtendeLogo.UseCases.Contracts.Identities;

namespace AtendeLogo.UseCases.Identities.Tenants.Services;

internal class TenantUserValidationService : ITenantUserValidationService
{
    private readonly ITenantUserRepository _tenantUserRepository;

    public TenantUserValidationService(
        ITenantUserRepository tenantUserRepository)
    {
        _tenantUserRepository = tenantUserRepository;
    }

    public async Task<bool> IsEmailUniqueAsync(
        string email, 
        CancellationToken cancellationToken = default)
    {
        return !await _tenantUserRepository.EmailExistsAsync(email, cancellationToken);
    }

    public async Task<bool> IsEmailUniqueAsync(
        Guid currentUser_Id, 
        string email,
        CancellationToken cancellationToken = default)
    {
        return !await _tenantUserRepository.EmailExistsAsync(currentUser_Id, email, cancellationToken);
    }

    public async Task<bool> IsPhoneNumberUniqueAsync(
        string phoneNumber,
        CancellationToken cancellationToken = default)
    {
        return !await _tenantUserRepository.PhoneNumberExitsAsync(phoneNumber, cancellationToken);
    }

    public async Task<bool> IsPhoneNumberUniqueAsync(
        Guid currentUser_Id, 
        string number, 
        CancellationToken cancellationToken = default)
    {
        return !await _tenantUserRepository.PhoneNumberExitsAsync(currentUser_Id, number, cancellationToken);
    }
}
