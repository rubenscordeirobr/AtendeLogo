namespace AtendeLogo.Persistence.Identity.Repositories;

internal class SystemUserRepository : UserRepository<SystemUser>, ISystemUserRepository
{
    public SystemUserRepository(
        IdentityDbContext dbContext,
        IUserSessionAccessor userSessionAccessor,
        TrackingOption trackingOption = TrackingOption.NoTracking)
        : base(dbContext, userSessionAccessor, trackingOption)
    {
    }
}
