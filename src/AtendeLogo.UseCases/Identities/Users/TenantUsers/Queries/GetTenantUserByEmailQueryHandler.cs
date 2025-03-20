namespace AtendeLogo.UseCases.Identities.Users.TenantUsers.Queries;

public class GetTenantUserByEmailQueryHandler
    : GetQueryResultHandler<GetTenantUserByEmailQuery, TenantUserResponse>
{
    private readonly ITenantUserRepository _tenantUserRepository;
    public GetTenantUserByEmailQueryHandler(
        ITenantUserRepository tenantUserRepository)
    {
        _tenantUserRepository = tenantUserRepository;
    }
    public override async Task<Result<TenantUserResponse>> HandleAsync(
        GetTenantUserByEmailQuery query,
        CancellationToken cancellationToken = default)
    {
        var user = await _tenantUserRepository.GetByEmailAsync(query.Email, cancellationToken);
        if (user is null)
        {
            return Result.NotFoundFailure<TenantUserResponse>(
                "TenantUser.NotFound",
                $"TenantUser with email {query.Email} not found.");
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
            Role = user.Role
        });
    }
}
