using AtendeLogo.Application.Contracts.Persistence.Identity;
using AtendeLogo.Common;

namespace AtendeLogo.UseCases.Identities.Users.TenantUsers.Queries;

public class GetTenantUserByEmailQueryHandler
    : SingleResultQueryHandler<GetTenantUserByEmailQuery, TenantUserResponse>
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
                "TenantUser with email {EntityId} not found.", query.Email);
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
