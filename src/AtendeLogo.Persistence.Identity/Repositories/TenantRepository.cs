using AtendeLogo.Application.Contracts.Persistence.Identity;
using AtendeLogo.Persistence.Common;
using AtendeLogo.Persistence.Common.Enums;

namespace AtendeLogo.Persistence.Identity.Repositories;

internal class TenantRepository : RepositoryBase<Tenant>, ITenantRepository
{
    public TenantRepository(
        IdentityDbContext dbContext,
        IRequestUserSessionService userSessionService,
        TrackingOption trackingOption = TrackingOption.NoTracking)
        : base(dbContext, userSessionService, trackingOption)
    {
    }

    public Task<Tenant?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken)
        => FindAsync(x => x.Name == name, cancellationToken);

    public Task<bool> EmailExistsAsync(
        string email,
        CancellationToken token)
    {
        return AnyAsync(x => x.Email == email, token);
    }

    public Task<bool> EmailExistsAsync(
        Guid currentTenant_Id,
        string email, CancellationToken token)
    {
        return AnyAsync(x => x.Email == email && x.Id != currentTenant_Id, token);
    }

    public Task<bool> FiscalCodeExistsAsync(
        string fiscalCode,
        CancellationToken token)
    {
        return AnyAsync(x => x.FiscalCode == fiscalCode, token);
    }

    public Task<bool> FiscalCodeExistsAsync(
        Guid currentTenant_Id,
        string fiscalCode, CancellationToken token)
    {
        return AnyAsync(x => x.FiscalCode == fiscalCode && x.Id != currentTenant_Id, token);
    }
}
