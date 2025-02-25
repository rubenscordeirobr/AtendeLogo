using AtendeLogo.Application.Contracts.Persistence.Identity;
using AtendeLogo.Common;

namespace AtendeLogo.UseCases.Identities.Users.AdminUsers.Queries;

public class GetAdminUserByEmailQueryHandler
    : SingleResultQueryHandler<GetAdminUserByEmailQuery, AdminUserResponse>
{
    private readonly IAdminUserRepository _adminUserRepository;

    public GetAdminUserByEmailQueryHandler(
        IAdminUserRepository adminUserRepository)
    {
        _adminUserRepository = adminUserRepository;
    }

    public override async Task<Result<AdminUserResponse>> HandleAsync(
        GetAdminUserByEmailQuery query,
        CancellationToken cancellationToken = default)
    {
        var user = await _adminUserRepository.GetByEmailAsync(query.Email, cancellationToken);
        if (user is null)
        {
            return Result.NotFoundFailure<AdminUserResponse>(
                "SystemUser.NotFound",
                "SystemUser with email {EntityId} not found.", query.Email);
        }
        return Result.Success(new AdminUserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            UserState = user.UserState,
            UserStatus = user.UserStatus,
            Role = user.AdminUserRole
        });
    }
}
