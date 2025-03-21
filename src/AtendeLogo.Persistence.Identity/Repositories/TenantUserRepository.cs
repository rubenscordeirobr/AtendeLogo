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
        CancellationToken cancellationToken = default)
    {
        return AnyAsync(x => x.Email == email && x.Id != currentTenantUser_Id, cancellationToken);
    }

    public Task<bool> EmailOrPhoneNumberExits(
        string emailOrPhoneNumber,
        CancellationToken cancellationToken = default)
    {
        return AnyAsync(x => x.Email == emailOrPhoneNumber || x.PhoneNumber.Number == emailOrPhoneNumber, cancellationToken);
    }

    public Task<bool> PhoneNumberExitsAsync(
        string phoneNumber,
        CancellationToken cancellationToken = default)
    {
        return AnyAsync(x => x.PhoneNumber.Number == phoneNumber, cancellationToken);
    }

    public Task<bool> PhoneNumberExitsAsync(
        Guid currentTenantUser_Id,
        string phoneNumber,
        CancellationToken cancellationToken = default)
    {
        return AnyAsync(x => x.PhoneNumber.Number == phoneNumber && x.Id != currentTenantUser_Id, cancellationToken);
    }

    #region Overrides
    protected override IQueryable<TenantUser> CreateQuery(
        Expression<Func<TenantUser, object?>>[]? includeExpressions)
    {
        var query = base.CreateQuery(includeExpressions);

        var userSession = GetCurrentSession();
        if (!userSession.IsTenantUser() && !userSession.IsSystemAdminUser())
        {
            return query.Take(1);
        }
        return query;
    }

    protected override bool ShouldFilterTenantOwned()
    {
        var endpointInstance = GetCurrentEndpointInstance();
        if (endpointInstance?.ServiceRole == ServiceRole.UserAuthentication)
        {
            return false;
        }
        return base.ShouldFilterTenantOwned();
    }

    #endregion
}
