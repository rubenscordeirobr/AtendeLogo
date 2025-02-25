using AtendeLogo.Common;
using AtendeLogo.UseCases.Identities.Users.TenantUsers.Queries;

namespace AtendeLogo.UseCases.Identities.Users.TenantUsers.Services;

public interface ITenantUserService
{
    Task<Result<TenantUserResponse>> GetTenantUserByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<Result<TenantUserResponse>> GetTenantUserByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    Task<Result<TenantUserResponse>> GetTenantUserByPhoneNumberAsync(
        string phoneNumber,
        CancellationToken cancellationToken = default);
}
