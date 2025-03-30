
using AtendeLogo.Application.Extensions;
using AtendeLogo.UseCases.Mappers.Identities;

namespace AtendeLogo.UseCases.Identities.Users.SystemUsers.Queries;

internal sealed partial class GetAllSystemUsersQueryHandler
    : IGetManyQueryHandler<GetAllSystemUsersQuery, UserResponse>
{
    private readonly ISystemUserRepository _systemUserRepository;
    public GetAllSystemUsersQueryHandler(
        ISystemUserRepository systemUserRepository)
    {
        _systemUserRepository = systemUserRepository;
    }

    public async Task<Result<IReadOnlyList<UserResponse>>> HandleAsync(
        GetAllSystemUsersQuery request,
        CancellationToken cancellationToken = default)
    {
        var users = await _systemUserRepository.GetAllAsync(cancellationToken);
        var userResponses = users.Select(UserMapper.ToResponse).ToList();
        return this.Success(userResponses);
    }
}
