using AtendeLogo.Application.Contracts.Persistence.Identity;
using AtendeLogo.Common;
namespace AtendeLogo.UseCases.Identities.Users.SystemUsers.Queries;

public sealed class GetSystemUserByIdQueryHandler
    : SingleResultQueryHandler<GetSystemUserByIdQuery, SystemUserResponse>
{
    private readonly ISystemUserRepository _systemUserRepository;
    public GetSystemUserByIdQueryHandler(
        ISystemUserRepository systemUserRepository)
    {
        _systemUserRepository = systemUserRepository;
    }

    public override async Task<Result<SystemUserResponse>> HandleAsync(
        GetSystemUserByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var user = await _systemUserRepository.GetByIdAsync(query.Id, cancellationToken);
        if (user is null)
        {
            return Result.NotFoundFailure<SystemUserResponse>(
                "SystemUser.NotFound",
                $"SystemUser with id {query.Id} not found.");
        }

        return Result.Success(new SystemUserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            UserState = user.UserState,
            UserStatus = user.UserStatus
        });
    }
}
