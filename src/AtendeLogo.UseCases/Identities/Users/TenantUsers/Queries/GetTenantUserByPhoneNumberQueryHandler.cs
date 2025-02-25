using AtendeLogo.Application.Contracts.Persistence.Identity;
using AtendeLogo.Common;

namespace AtendeLogo.UseCases.Identities.Users.TenantUsers.Queries;

public class GetTenantUserByPhoneNumberQueryHandler
    : SingleResultQueryHandler<GetTenantUserByPhoneNumberQuery, TenantUserResponse>
{
    private readonly ITenantUserRepository _tenantUserRepository;
    public GetTenantUserByPhoneNumberQueryHandler(
        ITenantUserRepository tenantUserRepository)
    {
        _tenantUserRepository = tenantUserRepository;
    }
    public override async Task<Result<TenantUserResponse>> HandleAsync(
        GetTenantUserByPhoneNumberQuery query,
        CancellationToken cancellationToken = default)
    {
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
            PhoneNumber = user.PhoneNumber,
            UserState = user.UserState,
            UserStatus = user.UserStatus,
            Role = user.TenantUserRole
        });
    }
}
