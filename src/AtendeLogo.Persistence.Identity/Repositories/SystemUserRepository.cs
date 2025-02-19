using AtendeLogo.Application.Contracts.Persistence.Identity;
using AtendeLogo.Persistence.Common.Enums;

namespace AtendeLogo.Persistence.Identity.Repositories;

internal class SystemUserRepository : UserRepository<SystemUser>, ISystemUserRepository
{
    public SystemUserRepository(
        IdentityDbContext dbContext,
        IRequestUserSessionService userSessionService,
        TrackingOption trackingOption = TrackingOption.NoTracking)
        : base(dbContext, userSessionService, trackingOption)
    {
    }
}
