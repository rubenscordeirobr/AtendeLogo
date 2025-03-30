namespace AtendeLogo.Persistence.Identity.Repositories;

internal class TenantUserRepository : UserRepository<TenantUser>, ITenantUserRepository
{
    public TenantUserRepository(
        IdentityDbContext dbContext,
        IHttpContextSessionAccessor userSessionAccessor,
        TrackingOption trackingOption = TrackingOption.NoTracking)
        : base(dbContext, userSessionAccessor, trackingOption)
    {
    }

    

    #region Overrides
    protected override IQueryable<TenantUser> CreateQuery(
        Expression<Func<TenantUser, object?>>[]? includeExpressions)
    {
        var query = base.CreateQuery(includeExpressions);

        if (!UserSession.IsTenantUser() && !UserSession.IsSystemAdminUser())
        {
            return query.Take(1);
        }
        return query;
    }

    protected override bool ShouldFilterTenantOwned()
    {
        if (EndpointInstance?.ServiceRole is ServiceRole.Authentication or ServiceRole.Validation )
        {
            return false;
        }
        return base.ShouldFilterTenantOwned();
    }

    #endregion
}
