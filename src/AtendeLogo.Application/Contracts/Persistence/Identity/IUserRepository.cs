namespace AtendeLogo.Application.Contracts.Persistence.Identity;

public interface IUserRepository<TUserEntity> : IRepositoryBase<TUserEntity>
    where TUserEntity : User
{
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken);
    Task<bool> EmailExistsAsync(string email, Guid userId, CancellationToken cancellationToken);
    Task<bool> PhoneNumberExistsAsync(string phoneNumber, CancellationToken cancellationToken);
    Task<bool> PhoneNumberExistsAsync(string phoneNumber, Guid userId, CancellationToken cancellationToken);
    Task<TUserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<TUserEntity?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken);
}
