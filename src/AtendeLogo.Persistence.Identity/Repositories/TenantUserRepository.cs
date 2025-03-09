using AtendeLogo.Domain.Extensions;

namespace AtendeLogo.Persistence.Identity.Repositories;

internal class TenantUserRepository : UserRepository<TenantUser>, ITenantUserRepository
{
    
    public TenantUserRepository(
        IdentityDbContext dbContext,
        IUserSessionAccessor userSessionAccessor,
        TrackingOption trackingOption = TrackingOption.NoTracking)
        : base(dbContext, userSessionAccessor, trackingOption)
    {
    }

    public Task<bool> EmailExistsAsync(
        Guid currentTenantUser_Id,
        string email,
        CancellationToken token)
    {
        return AnyAsync(x => x.Email == email && x.Id != currentTenantUser_Id, token);
    }

    public Task<bool> EmailOrPhoneNumberExits(
        string emailOrPhoneNumber,
        CancellationToken token)
    {
        return AnyAsync(x => x.Email == emailOrPhoneNumber || x.PhoneNumber.Number == emailOrPhoneNumber, token);
    }

    #region Overrides
    protected override IQueryable<TenantUser> GetInitialQuery()
    {
        var query = base.GetInitialQuery();
        var userSession = _userSessionAccessor.GetCurrentSession();
        if (!userSession.IsTenantUser() && !userSession.IsSystemAdminUser())
        {
            return query.Take(1);
        }
        return query;
    }

    protected override bool ShouldFilterTenantOwned()
    {
        var endpointInstance = _userSessionAccessor.GetCurrentEndpointInstance();
        if(endpointInstance?.ServiceRole == ServiceRole.UserAuthentication)
        {
            return false;
        }
        return base.ShouldFilterTenantOwned();
    }
 
    #endregion
}
