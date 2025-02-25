using AtendeLogo.Common;
using AtendeLogo.UseCases.Identities.Users.AdminUsers.Queries;

namespace AtendeLogo.UseCases.Identities.Users.AdminUsers.Services;

public interface IAdminUserService
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
