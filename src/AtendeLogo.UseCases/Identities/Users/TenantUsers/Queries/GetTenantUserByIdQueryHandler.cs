namespace AtendeLogo.UseCases.Identities.Users.TenantUsers.Queries;

public class GetTenantUserByIdQueryHandler
    : SingleResultQueryHandler<GetTenantUserByIdQuery, TenantUserResponse>
{
    private readonly ITenantUserRepository _tenantUserRepository;
    public GetTenantUserByIdQueryHandler(
        ITenantUserRepository tenantUserRepository)
    {
        _tenantUserRepository = tenantUserRepository;
    }

    public override async Task<Result<TenantUserResponse>> HandleAsync(
        GetTenantUserByIdQuery query, 
        CancellationToken cancellationToken = default)
    {
        var user = await _tenantUserRepository.GetByIdAsync(query.Id, cancellationToken);
        if (user is null)
        {
            return Result.NotFoundFailure<TenantUserResponse>(
                "TenantUser.NotFound",
                $"TenantUser with id {query.Id} not found.");
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
            EmailVerificationState = user.EmailVerificationState,
            PhoneNumberVerificationState = user.PhoneNumberVerificationState,
            Role = user.Role
        });
    }
}

