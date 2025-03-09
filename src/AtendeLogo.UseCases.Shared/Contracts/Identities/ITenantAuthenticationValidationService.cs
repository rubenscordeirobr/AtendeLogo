namespace AtendeLogo.UseCases.Contracts.Identities;

public interface ITenantAuthenticationValidationService : IValidationService
{
    Task<bool> EmailOrPhoneNumberExitsAsync(
        string emailOrPhoneNumber, 
        CancellationToken cancellationToken);

    Task<bool> VerifyTenantUserCredentialsAsync(
        string emailOrPhoneNumber,
        string password,
        CancellationToken cancellationToken);
}
