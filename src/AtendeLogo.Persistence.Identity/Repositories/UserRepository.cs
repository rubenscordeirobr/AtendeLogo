namespace AtendeLogo.Persistence.Identity.Repositories;

internal abstract class UserRepository<TUserEntity> : RepositoryBase<TUserEntity>, IUserRepository<TUserEntity>
    where TUserEntity : User
{
    protected override int DefaultMaxRecords
        => GetDefaultMaxRecords();

    public UserRepository(IdentityDbContext dbContext,
        IUserSessionAccessor userSessionAccessor,
        TrackingOption trackingOption = TrackingOption.NoTracking)
        : base(dbContext, userSessionAccessor, trackingOption)
    {
    }

    public Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
        => AnyAsync(x => x.Email == email, cancellationToken);

    public Task<bool> EmailExistsAsync(string email, Guid user_Id, CancellationToken cancellationToken = default)
        => AnyAsync(x => x.Email == email && x.Id != user_Id, cancellationToken);

    public Task<bool> PhoneNumberExistsAsync(string phoneNumber, CancellationToken cancellationToken = default)
        => AnyAsync(x => x.PhoneNumber.Number == phoneNumber, cancellationToken);

    public Task<bool> PhoneNumberExistsAsync(string phoneNumber, Guid user_Id, CancellationToken cancellationToken = default)
        => AnyAsync(x => x.PhoneNumber.Number == phoneNumber && x.Id != user_Id, cancellationToken);

    public Task<TUserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => FindAsync(x => x.Email == email, cancellationToken);

    public Task<TUserEntity?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
        => FindAsync(x => x.PhoneNumber.Number == phoneNumber, cancellationToken);

    public Task<TUserEntity?> GetByEmailOrPhoneNumberAsync(
        string emailOrPhoneNumber,
        CancellationToken cancellationToken = default)
        => FindAsync(x => x.Email == emailOrPhoneNumber || x.PhoneNumber.Number == emailOrPhoneNumber, cancellationToken);

    private int GetDefaultMaxRecords()
    {
        var userSession = _userSessionAccessor.GetCurrentSession();
        return (!userSession.IsTenantUser() && !userSession.IsSystemAdminUser())
            ? 1
            : base.DefaultMaxRecords;
    }
}

