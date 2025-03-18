namespace AtendeLogo.Application.Contracts.Persistence.Identities;

public interface ITenantRepository : IRepositoryBase<Tenant>
{
    Task<Tenant?> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task<bool> EmailExistsAsync(string email, CancellationToken token);
    Task<bool> EmailExistsAsync(Guid currentTenant_Id, string email, CancellationToken token);
    Task<bool> FiscalCodeExistsAsync( string fiscalCode, CancellationToken token);
    Task<bool> FiscalCodeExistsAsync(Guid currentTenant_Id, string fiscalCode, CancellationToken token);
    Task<bool> PhoneNumberExitsAsync(string phoneNumber, CancellationToken token);
    Task<bool> PhoneNumberExitsAsync(Guid currentTenant_Id, string phoneNumber, CancellationToken token);
}
