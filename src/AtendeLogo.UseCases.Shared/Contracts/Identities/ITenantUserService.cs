using AtendeLogo.UseCases.Identities.Users.TenantUsers.Queries;

namespace AtendeLogo.UseCases.Contracts.Identities;

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

    Task<Result<TenantUserResponse>> GetTenantUserByEmailOrPhoneNumberAsync(
        string emailOrPhoneNumber,
        CancellationToken cancellationToken = default);
}
