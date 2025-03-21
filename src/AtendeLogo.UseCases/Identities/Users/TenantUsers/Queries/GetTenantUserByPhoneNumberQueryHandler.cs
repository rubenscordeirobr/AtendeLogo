namespace AtendeLogo.UseCases.Identities.Users.TenantUsers.Queries;

public class GetTenantUserByPhoneNumberQueryHandler
    : IGetQueryResultHandler<GetTenantUserByPhoneNumberQuery, TenantUserResponse>
{
    private readonly ITenantUserRepository _tenantUserRepository;
    public GetTenantUserByPhoneNumberQueryHandler(
        ITenantUserRepository tenantUserRepository)
    {
        Guard.NotNull(tenantUserRepository);

        _tenantUserRepository = tenantUserRepository;
    }

    public async Task<Result<TenantUserResponse>> HandleAsync(
        GetTenantUserByPhoneNumberQuery query,
        CancellationToken cancellationToken = default)
    {
        Guard.NotNull(query);

        var user = await _tenantUserRepository.GetByPhoneNumberAsync(query.PhoneNumber, cancellationToken);
        if (user is null)
        {
            return Result.NotFoundFailure<TenantUserResponse>(
                "TenantUser.NotFound",
                $"TenantUser with phone number {query.PhoneNumber} not found.");
        }
        return Result.Success(new TenantUserResponse
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
