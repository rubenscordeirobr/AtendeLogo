using AtendeLogo.Application.Contracts.Persistence.Identity;
using AtendeLogo.Persistence.Common.Enums;

namespace AtendeLogo.Persistence.Identity.Repositories;

internal class TenantUserRepository : UserRepository<TenantUser>, ITenantUserRepository
{
    public TenantUserRepository(
        IdentityDbContext dbContext,
        IUserSessionService userSessionService,
        TrackingOption trackingOption = TrackingOption.NoTracking)
        : base(dbContext, userSessionService, trackingOption)
    {
    }
}
