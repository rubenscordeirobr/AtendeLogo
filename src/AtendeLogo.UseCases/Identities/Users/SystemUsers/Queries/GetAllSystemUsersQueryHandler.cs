using AtendeLogo.Application.Contracts.Persistence.Identity;

namespace AtendeLogo.UseCases.Identities.Users.SystemUsers.Queries;
internal sealed partial class GetAllSystemUsersQueryHandler
    : CollectionQueryHandler<GetAllSystemUsersQuery, SystemUserResponse>
{
    private readonly ISystemUserRepository _systemUserRepository;
    public GetAllSystemUsersQueryHandler(
        ISystemUserRepository systemUserRepository)
    {
        _systemUserRepository = systemUserRepository; ;
    }
    public override async Task<IReadOnlyList<SystemUserResponse>> HandleAsync(
        GetAllSystemUsersQuery request,
        CancellationToken cancellationToken = default)
    {

        var users = await _systemUserRepository.GetAllAsync(cancellationToken);

        return users.Select(user => new SystemUserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            UserState = user.UserState,
            UserStatus = user.UserStatus
        }).ToList();
    }
}
