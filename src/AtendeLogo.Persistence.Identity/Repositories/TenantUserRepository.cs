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
}
