using AtendeLogo.UseCases.Identities.Users.AdminUsers.Queries;

namespace AtendeLogo.UseCases.Contracts.Identities;

public interface IAdminUserService : IEndpointService
{
    Task<Result<AdminUserResponse>> GetAdminUserByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<Result<AdminUserResponse>> GetAdminUserByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    Task<Result<AdminUserResponse>> GetAdminUserByPhoneNumberAsync(
        string phoneNumber,
        CancellationToken cancellationToken = default);

    Task<Result<AdminUserResponse>> GetAdminUserByEmailOrPhoneNumberAsync(
        string emailOrPhoneNumber,
        CancellationToken cancellationToken = default);
}
