namespace AtendeLogo.UseCases.Contracts.Identities;
public interface ITenantValidationService: IValidationService
{
    Task<bool> IsEmailUniqueAsync(
        string email, 
        CancellationToken token);
   
    Task<bool> IsEmailUniqueAsync(
        Guid currentTenant_Id,
        Guid currentTenantOwner_Id,
        string email, 
        CancellationToken token);

    Task<bool> IsFiscalCodeUniqueAsync(
        string fiscalCode, 
        CancellationToken token);
  
    Task<bool> IsFiscalCodeUniqueAsync(
        Guid currentTenant_Id,
        string fiscalCodel,
        CancellationToken token);
}
