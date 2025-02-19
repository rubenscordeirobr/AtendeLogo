using AtendeLogo.Application.Contracts.Persistence.Identity;
using AtendeLogo.Common;

namespace AtendeLogo.UseCases.Identities.Users.AdminUsers.Queries;

public class GetAdminUserByIdQueryHandler
     : SingleResultQueryHandler<GetAdminUserByIdQuery, AdminUserResponse>
{

    private readonly IAdminUserRepository _adminUserRepository;
    public GetAdminUserByIdQueryHandler(
        IAdminUserRepository adminUserRepository)
    {
        _adminUserRepository = adminUserRepository;

    }
    public override async Task<Result<AdminUserResponse>> HandleAsync(
        GetAdminUserByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var user = await _adminUserRepository.GetByIdAsync(query.Id, cancellationToken);
        if (user is null)
        {
            return Result.NotFoundFailure<AdminUserResponse>(
                "SystemUser.NotFound",
                "SystemUser with id {EntityId} not found.", query.Id);
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

