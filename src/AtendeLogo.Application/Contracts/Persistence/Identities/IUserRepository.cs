namespace AtendeLogo.Application.Contracts.Persistence.Identities;

public interface IUserRepository<TUser> : IRepositoryBase<TUser>
    where TUser : User
{
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, Guid user_Id, CancellationToken cancellationToken = default);
    Task<bool> PhoneNumberExistsAsync(string phoneNumber, CancellationToken cancellationToken = default);
    Task<bool> PhoneNumberExistsAsync(string phoneNumber, Guid user_Id, CancellationToken cancellationToken = default);
    Task<TUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<TUser?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);
    Task<TUser?> GetByEmailOrPhoneNumberAsync(string emailOrPhoneNumber, CancellationToken cancellationToken = default);
}
