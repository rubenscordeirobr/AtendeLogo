using AtendeLogo.Application.Contracts.Persistence.Identity;
using AtendeLogo.Persistence.Common;
using AtendeLogo.Persistence.Common.Enums;

namespace AtendeLogo.Persistence.Identity.Repositories;
internal class UserSessionRepository : RepositoryBase<UserSession>, IUserSessionRepository
{
    public UserSessionRepository(
        IdentityDbContext dbContext, 
        IUserSessionService userSessionService,
        TrackingOption trackingOption = TrackingOption.NoTracking)
        : base(dbContext, userSessionService, trackingOption)
    {
    }

    public Task<UserSession?> GetByClientTokenAsync(string sessionToken)
        => FindAsync(x => x.ClientSessionToken == sessionToken);
}
