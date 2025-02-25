using AtendeLogo.Common;
using AtendeLogo.UseCases.Identities.Users.AdminUsers.Queries;

namespace AtendeLogo.UseCases.Identities.Users.AdminUsers.Services;

public interface IAdminUserService
{
    Task<Result<AdminUserResponse>> GetAdminUserByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
