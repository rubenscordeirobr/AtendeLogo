namespace AtendeLogo.Persistence.Identity.Repositories;

internal class AdminUserRepository : UserRepository<AdminUser>, IAdminUserRepository
{
    public AdminUserRepository(
        IdentityDbContext dbContext,
        IUserSessionAccessor userSessionAccessor,
        TrackingOption trackingOption = TrackingOption.NoTracking)
        : base(dbContext, userSessionAccessor, trackingOption)
    {
    }
}
