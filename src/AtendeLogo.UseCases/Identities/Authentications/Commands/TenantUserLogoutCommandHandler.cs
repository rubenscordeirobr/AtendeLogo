namespace AtendeLogo.UseCases.Identities.Authentications.Commands;

public class TenantUserLogoutCommandHandler : CommandHandler<TenantUserLogoutCommand, TenantUserLogoutResponse>
{
    private readonly IIdentityUnitOfWork _unitOfWork;
    private readonly IUserSessionAccessor _userSessionAccessor;
    private readonly ISessionCacheService _sessionCacheService;

    public TenantUserLogoutCommandHandler(
        IIdentityUnitOfWork unitOfWork,
        IUserSessionAccessor userSessionAccessor,
        ISessionCacheService sessionCacheService)
    {
        _unitOfWork = unitOfWork;
        _userSessionAccessor = userSessionAccessor;
        _sessionCacheService = sessionCacheService;
    }

    protected override async Task<Result<TenantUserLogoutResponse>> HandleAsync(
        TenantUserLogoutCommand command,
        CancellationToken cancellationToken)
    {
        var clientSessionToken = command.ClientSessionToken;
        var userSession = await _unitOfWork.UserSessions.GetByClientTokenAsync(
            command.ClientSessionToken, cancellationToken);

        if (userSession is null)
        {
            return Result.NotFoundFailure<TenantUserLogoutResponse>(
                "UserSession.NotFound",
                $"UserSession with token {clientSessionToken} not found.");
        }

        if (userSession.IsActive)
        {
            userSession.TerminateSession(SessionTerminationReason.UserLogout);
            _unitOfWork.Update(userSession);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        _userSessionAccessor.RemoveClientSessionCookie(clientSessionToken);
        await _sessionCacheService.RemoveSessionAsync(clientSessionToken, cancellationToken);
        return Result.Success(new TenantUserLogoutResponse());
    }
}

