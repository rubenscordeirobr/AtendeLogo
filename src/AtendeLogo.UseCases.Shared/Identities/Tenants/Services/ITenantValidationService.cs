using AtendeLogo.UseCases.Common.Services;

namespace AtendeLogo.UseCases.Identities.Tenants.Services;
public interface ITenantValidationService: IValidationService
{
    Task<bool> IsEmailUniqueAsync(string email, CancellationToken token);
    Task<bool> IsEmailUniqueAsync(Guid currentTenant_Id, string email, CancellationToken token);

    Task<bool> IsFiscalCodeUniqueAsync(string fiscalCode, CancellationToken token);
    Task<bool> IsFiscalCodeUniqueAsync(Guid currentTenant_Id, string fiscalCode, CancellationToken token);
}
