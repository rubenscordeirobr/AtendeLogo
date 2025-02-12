namespace AtendeLogo.Application.Contracts.Persistence.Identity;

public interface ITenantRepository : IRepositoryBase<Tenant>
{
    Task<Tenant?> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task<bool> EmailExistsAsync(string email, CancellationToken token);
    Task<bool> EmailExistsAsync(Guid currentTenant_Id, string email, CancellationToken token);
}
