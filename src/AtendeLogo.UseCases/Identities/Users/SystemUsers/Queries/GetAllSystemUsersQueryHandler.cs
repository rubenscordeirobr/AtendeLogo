﻿
namespace AtendeLogo.UseCases.Identities.Users.SystemUsers.Queries;

internal sealed partial class GetAllSystemUsersQueryHandler
    : GetManyQueryHandler<GetAllSystemUsersQuery, SystemUserResponse>
{
    private readonly ISystemUserRepository _systemUserRepository;
    public GetAllSystemUsersQueryHandler(
        ISystemUserRepository systemUserRepository)
    {
        _systemUserRepository = systemUserRepository; ;
    }
    public override async Task<Result<IReadOnlyList<SystemUserResponse>>> HandleAsync(
        GetAllSystemUsersQuery request,
        CancellationToken cancellationToken = default)
    {
        var users = await _systemUserRepository.GetAllAsync(cancellationToken);

        var userResponses = users.Select(user => new SystemUserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            ProfilePictureUrl = user.ProfilePictureUrl,
            Role = user.Role,
            Language = user.Language,
            PhoneNumber = user.PhoneNumber,
            UserState = user.UserState,
            UserStatus = user.UserStatus,
            UserType = user.UserType,
            EmailVerificationState = user.EmailVerificationState,
            PhoneNumberVerificationState = user.PhoneNumberVerificationState
        }).ToList();

        return Success(userResponses);
    }
}
