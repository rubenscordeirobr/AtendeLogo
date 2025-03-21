using AtendeLogo.Application.Contracts.Security;
using AtendeLogo.Common.Helpers;
using AtendeLogo.UseCases.Contracts.Identities;

namespace AtendeLogo.UseCases.Identities.Authentications.Services;

internal class TenantAuthenticationValidationService : ITenantAuthenticationValidationService
{
    private readonly ITenantUserRepository _tenantUserRepository;
    private readonly ISecureConfiguration _secureConfiguration;
     
    public TenantAuthenticationValidationService(
        ITenantUserRepository tenantRepository,
        ISecureConfiguration secureConfiguration)
    {
        _tenantUserRepository = tenantRepository;
        _secureConfiguration = secureConfiguration;
    }

    public async Task<bool> VerifyTenantUserCredentialsAsync(
        string emailOrPhoneNumber,
        string password, 
        CancellationToken cancellationToken = default)
    {
        var tenantUser = await _tenantUserRepository.GetByEmailOrPhoneNumberAsync(emailOrPhoneNumber, cancellationToken);
        if (tenantUser == null)
        {
            return false;
        }

        var salt = _secureConfiguration.GetPasswordSalt();
        return PasswordHelper.VerifyPassword(
            password,
            tenantUser.Password.HashValue,
            salt);
    }

    public Task<bool> EmailOrPhoneNumberExitsAsync(
        string emailOrPhoneNumber,
        CancellationToken cancellationToken = default)
    {
        return _tenantUserRepository.EmailOrPhoneNumberExits(emailOrPhoneNumber, cancellationToken);
    }
     
}
