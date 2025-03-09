
namespace AtendeLogo.Persistence.Identity.Repositories;
internal class UserSessionRepository : RepositoryBase<UserSession>, IUserSessionRepository
{
    public UserSessionRepository(
        IdentityDbContext dbContext,
        IUserSessionAccessor userSessionAccessor,
        TrackingOption trackingOption = TrackingOption.NoTracking)
        : base(dbContext, userSessionAccessor, trackingOption)
    {
    }

    public Task<UserSession?> GetByClientTokenAsync(
        string sessionToken, 
        CancellationToken cancellationToken = default)
        => FindAsync(x => x.ClientSessionToken == sessionToken, cancellationToken);
 
}
