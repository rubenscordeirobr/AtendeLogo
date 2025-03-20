namespace AtendeLogo.UseCases.Identities.Users.AdminUsers.Queries;

public class GetAdminUserByIdQueryHandler
     : GetQueryResultHandler<GetAdminUserByIdQuery, AdminUserResponse>
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
                $"SystemUser with id {query.Id} not found.");
        }

        return Result.Success(new AdminUserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            ProfilePictureUrl = user.ProfilePictureUrl,
            Language = user.Language,
            UserState = user.UserState,
            UserStatus = user.UserStatus,
            UserType = user.UserType,
            EmailVerificationState = user.EmailVerificationState,
            PhoneNumberVerificationState = user.PhoneNumberVerificationState,
            Role = user.Role,
            PhoneNumber = user.PhoneNumber,
        });
    }
}

