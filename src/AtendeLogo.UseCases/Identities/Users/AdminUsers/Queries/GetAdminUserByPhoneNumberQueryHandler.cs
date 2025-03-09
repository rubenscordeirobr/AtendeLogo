namespace AtendeLogo.UseCases.Identities.Users.AdminUsers.Queries;

public class GetAdminUserByPhoneNumberQueryHandler
    : SingleResultQueryHandler<GetAdminUserByPhoneNumberQuery, AdminUserResponse>
{
    private readonly IAdminUserRepository _adminUserRepository;
    public GetAdminUserByPhoneNumberQueryHandler(
        IAdminUserRepository adminUserRepository)
    {
        _adminUserRepository = adminUserRepository;
    }
    public override async Task<Result<AdminUserResponse>> HandleAsync(
        GetAdminUserByPhoneNumberQuery query,
        CancellationToken cancellationToken = default)
    {
        var user = await _adminUserRepository.GetByPhoneNumberAsync(query.PhoneNumber, cancellationToken);
        if (user is null)
        {
            return Result.NotFoundFailure<AdminUserResponse>(
                "SystemUser.NotFound",
                $"SystemUser with phone number {query.PhoneNumber} not found.");
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
            EmailVerificationState = user.EmailVerificationState,
            PhoneNumberVerificationState = user.PhoneNumberVerificationState,
            Role = user.Role,
            PhoneNumber = user.PhoneNumber,
        });
    }
}
