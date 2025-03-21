namespace AtendeLogo.UseCases.Identities.Users.TenantUsers.Queries;

public class GetTenantUserByEmailOrPhoneNumberQueryHandler
    : IGetQueryResultHandler<GetTenantUserByEmailOrPhoneNumberQuery, TenantUserResponse>
{
    private readonly ITenantUserRepository _tenantUserRepository;
    public GetTenantUserByEmailOrPhoneNumberQueryHandler(
        ITenantUserRepository tenantUserRepository)
    {
        _tenantUserRepository = tenantUserRepository;
    }
    public async Task<Result<TenantUserResponse>> HandleAsync(
        GetTenantUserByEmailOrPhoneNumberQuery query,
        CancellationToken cancellationToken = default)
    {
        Guard.NotNull(query);

        var user = await _tenantUserRepository.GetByEmailOrPhoneNumberAsync(query.EmailOrPhoneNumber, cancellationToken);
        if (user is null)
        {
            return Result.NotFoundFailure<TenantUserResponse>(
                "TenantUser.NotFound",
                $"TenantUser with email or phone number {query.EmailOrPhoneNumber} not found.");
        }
        return Result.Success(new TenantUserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            ProfilePictureUrl = user.ProfilePictureUrl,
            Language = user.Language,
            PhoneNumber = user.PhoneNumber,
            UserState = user.UserState,
            UserStatus = user.UserStatus,
            UserType = user.UserType,
            EmailVerificationState = user.EmailVerificationState,
            PhoneNumberVerificationState = user.PhoneNumberVerificationState,
            Role = user.Role,
        });
    }
}
