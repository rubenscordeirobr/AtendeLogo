namespace AtendeLogo.UseCases.Identities.Users.AdminUsers.Queries;

public class GetAdminUserByEmailOrPhoneNumberQueryHandler
    : IGetQueryResultHandler<GetAdminUserByEmailOrPhoneNumberQuery, AdminUserResponse>
{
    private readonly IAdminUserRepository _adminUserRepository;
    public GetAdminUserByEmailOrPhoneNumberQueryHandler(
        IAdminUserRepository adminUserRepository)
    {
        _adminUserRepository = adminUserRepository;
    }
    public async Task<Result<AdminUserResponse>> HandleAsync(
        GetAdminUserByEmailOrPhoneNumberQuery query,
        CancellationToken cancellationToken = default)
    {
        Guard.NotNull(query);

        var user = await _adminUserRepository.GetByEmailOrPhoneNumberAsync(query.EmailOrPhonenumber, cancellationToken);
        if (user is null)
        {
            return Result.NotFoundFailure<AdminUserResponse>(
                "SystemUser.NotFound",
                $"SystemUser with email or phone number  {query.EmailOrPhonenumber}  not found.");
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
            UserType = user.UserType,
            EmailVerificationState = user.EmailVerificationState,
            PhoneNumberVerificationState = user.PhoneNumberVerificationState,
            Role = user.Role
        });
    }
}
