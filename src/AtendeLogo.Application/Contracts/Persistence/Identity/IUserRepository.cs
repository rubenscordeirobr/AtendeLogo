namespace AtendeLogo.Application.Contracts.Persistence.Identity;

public interface IUserRepository<TUserEntity> : IRepositoryBase<TUserEntity>
    where TUserEntity : User
{
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, Guid userId, CancellationToken cancellationToken = default);
    Task<bool> PhoneNumberExistsAsync(string phoneNumber, CancellationToken cancellationToken = default);
    Task<bool> PhoneNumberExistsAsync(string phoneNumber, Guid userId, CancellationToken cancellationToken = default);
    Task<TUserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<TUserEntity?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);
}
