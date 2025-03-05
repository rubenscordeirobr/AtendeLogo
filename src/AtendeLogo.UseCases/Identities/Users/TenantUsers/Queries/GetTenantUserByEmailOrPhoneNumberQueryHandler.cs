namespace AtendeLogo.UseCases.Identities.Users.TenantUsers.Queries;

public class GetTenantUserByEmailOrPhoneNumberQueryHandler
    : SingleResultQueryHandler<GetTenantUserByEmailOrPhoneNumberQuery, TenantUserResponse>
{
    private readonly ITenantUserRepository _tenantUserRepository;
    public GetTenantUserByEmailOrPhoneNumberQueryHandler(
        ITenantUserRepository tenantUserRepository)
    {
        _tenantUserRepository = tenantUserRepository;
    }
    public override async Task<Result<TenantUserResponse>> HandleAsync(
        GetTenantUserByEmailOrPhoneNumberQuery query,
        CancellationToken cancellationToken = default)
    {
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
            PhoneNumber = user.PhoneNumber,
            UserState = user.UserState,
            UserStatus = user.UserStatus,
            Role = user.TenantUserRole
        });
    }
}
