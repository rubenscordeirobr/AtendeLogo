﻿using AtendeLogo.Application.Contracts.Persistence.Identity;
using AtendeLogo.Persistence.Common;
using AtendeLogo.Persistence.Common.Enums;

namespace AtendeLogo.Persistence.Identity.Repositories;

internal abstract class UserRepository<TUserEntity> : RepositoryBase<TUserEntity>, IUserRepository<TUserEntity>
    where TUserEntity : User
{
    public UserRepository(IdentityDbContext dbContext,
        IRequestUserSessionService userSessionService,
        TrackingOption trackingOption = TrackingOption.NoTracking)
        : base(dbContext, userSessionService, trackingOption)
    {
    }

    public Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
        => AnyAsync(x => x.Email == email, cancellationToken);

    public Task<bool> EmailExistsAsync(string email, Guid user_Id, CancellationToken cancellationToken = default)
        => AnyAsync(x => x.Email == email && x.Id != user_Id , cancellationToken);

    public Task<bool> PhoneNumberExistsAsync(string phoneNumber, CancellationToken cancellationToken = default)
        => AnyAsync(x => x.PhoneNumber.Number == phoneNumber, cancellationToken);

    public Task<bool> PhoneNumberExistsAsync(string phoneNumber, Guid user_Id, CancellationToken cancellationToken = default)
        => AnyAsync(x => x.PhoneNumber.Number == phoneNumber && x.Id != user_Id, cancellationToken);

    public Task<TUserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => FindAsync(x => x.Email == email, cancellationToken);

    public Task<TUserEntity?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
        => FindAsync(x => x.PhoneNumber.Number == phoneNumber, cancellationToken);

    public Task<TUserEntity?> GetByEmailOrPhoneNumberAsync(
        string email,
        string phoneNumber, 
        CancellationToken cancellationToken = default)
        => FindAsync(x => x.Email == email || x.PhoneNumber.Number == phoneNumber, cancellationToken);
}
