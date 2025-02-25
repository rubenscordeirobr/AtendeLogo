using AtendeLogo.Application.Contracts.Persistence.Identity;
using AtendeLogo.Common;

namespace AtendeLogo.UseCases.Identities.Users.AdminUsers.Queries;

public class GetAdminUserByEmailOrPhoneNumberQueryHandler
    : SingleResultQueryHandler<GetAdminUserByEmailOrPhoneNumberQuery, AdminUserResponse>
{
    private readonly IAdminUserRepository _adminUserRepository;
    public GetAdminUserByEmailOrPhoneNumberQueryHandler(
        IAdminUserRepository adminUserRepository)
    {
        _adminUserRepository = adminUserRepository;
    }
    public override async Task<Result<AdminUserResponse>> HandleAsync(
        GetAdminUserByEmailOrPhoneNumberQuery query,
        CancellationToken cancellationToken = default)
    {
        var user = await _adminUserRepository.GetByEmailOrPhoneNumberAsync(query.Email, query.PhoneNumber, cancellationToken);
        if (user is null)
        {
            return Result.NotFoundFailure<AdminUserResponse>(
                "SystemUser.NotFound",
                "SystemUser with email or phone number {EntityId} not found.", query.Email);
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
