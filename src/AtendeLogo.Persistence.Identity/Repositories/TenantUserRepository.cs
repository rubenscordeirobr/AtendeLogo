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
     
    public Task<bool> PhoneNumberExitsAsync(
        string phoneNumber, 
        CancellationToken token)
    {
        return AnyAsync(x => x.PhoneNumber.Number == phoneNumber, token);
    }

    public Task<bool> PhoneNumberExitsAsync(
        Guid currentTenantUser_Id, 
        string phoneNumber,
        CancellationToken token)
    {
        return AnyAsync(x => x.PhoneNumber.Number == phoneNumber && x.Id != currentTenantUser_Id, token);
    }

    #region Overrides
    protected override IQueryable<TenantUser> CreateQuery(
        Expression<Func<TenantUser, object?>>[] includeExpressions)
    {
        var query = base.CreateQuery(includeExpressions);
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
