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
                $"SystemUser with email {query.Email} not found.");
        }

        return Result.Success(new AdminUserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            ProfilePictureUrl = user.ProfilePictureUrl,
            PhoneNumber = user.PhoneNumber,
            Language = user.Language,
            UserState = user.UserState,
            UserStatus = user.UserStatus,
            EmailVerificationState = user.EmailVerificationState,
            PhoneNumberVerificationState = user.PhoneNumberVerificationState,
            Role = user.Role
        });
    }
}
