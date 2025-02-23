using System.Diagnostics;
using AtendeLogo.Application.Contracts.Persistence.Identity;
using AtendeLogo.Application.Contracts.Services;
using AtendeLogo.Application.Models.Identities;
using AtendeLogo.Common.Helpers;

namespace AtendeLogo.Application.Services;

public class UserSessionVerificationService : IUserSessionVerificationService, IAsyncDisposable
{
    private readonly ISessionCacheService _sessionCacheService;
    private readonly IIdentityUnitOfWork _unitWork;
    private readonly IRequestUserSessionService _userSessionService;

    private IUserSessionRepository UserSessionRepository
        => _unitWork.UserSessionRepository;

    public UserSessionVerificationService(
        ISessionCacheService cacheSessionService,
        IRequestUserSessionService userSessionService,
        IIdentityUnitOfWork unitWork)
    {
        _sessionCacheService = cacheSessionService;
        _userSessionService = userSessionService;
        _unitWork = unitWork;
    }

    public async Task<IUserSession> VerifyAsync()
    {
        var session = await RetrieveSessionAsync();
        _userSessionService.AddClientSessionCookie(session.ClientSessionToken);
        return session;

    }
    private async Task<IUserSession> RetrieveSessionAsync()
    {
        var userSession = await GetUserSessionAsync();
        if (userSession is not null)
        {
            await ValidateSessionAsync(userSession);

            if (userSession.IsActive)
            {
                return userSession;
            }
        }
        return await CreateAnonymousSessionAsync(userSession);
    }

    private async Task<IUserSession?> GetUserSessionAsync()
    {
        var clientSessionToken = _userSessionService.GetClientSessionToken();
        if (string.IsNullOrWhiteSpace(clientSessionToken))
            return null;

        var cachedSession = await GetSessionFromCacheAsync(clientSessionToken);
        if (cachedSession is not null)
        {
            return cachedSession;
        }
        return await UserSessionRepository.GetByClientTokenAsync(clientSessionToken);
    }

    private async Task<IUserSession?> GetSessionFromCacheAsync(string clientSessionToken)
    {
        var cachedSession = await _sessionCacheService.GetSessionAsync(clientSessionToken);
        if (cachedSession?.IsUpdatePending() == true)
        {
            return await UserSessionRepository.TryRefreshAsync(cachedSession);
        }
        return cachedSession;
    }

    private async Task ValidateSessionAsync(IUserSession userSession)
    {
        if (!userSession.IsActive)
            return;

        if (userSession is AnonymousUserSession)
        {
            return;
        }

        if (!(userSession is UserSession userSessionEntity))
        {
            throw new InvalidOperationException("Invalid session type.");
        }

        await ValidateSessionEntityAsync(userSessionEntity);
    }

    private async Task ValidateSessionEntityAsync(UserSession userSession)
    {
        var headerInfo = _userSessionService.GetRequestHeaderInfo();
        if (userSession.IsUpdatePending() || Debugger.IsAttached)
        {
            userSession.UpdateLastActivity();

            _unitWork.Update(userSession);

            var result = await _unitWork.SaveChangesAsync(silent: true);
            if (!result.IsSuccess)
            {
                if (result.Error is DomainEventError)
                {
                    await TerminateSessionAsync(userSession, SessionTerminationReason.DomainEventError);
                }
                else
                {
                    throw result.Exception;
                }
                return;
            }
            await _sessionCacheService.AddSessionAsync(userSession);
        }

        if (!string.Equals(userSession.IpAddress, headerInfo.IpAddress, StringComparison.OrdinalIgnoreCase))
        {
            await TerminateSessionAsync(userSession, SessionTerminationReason.IpAddressChanged);
            return;
        }

        if (!string.Equals(userSession.UserAgent, headerInfo.UserAgent, StringComparison.OrdinalIgnoreCase))
        {
            await TerminateSessionAsync(userSession, SessionTerminationReason.UserAgentChanged);
            return;
        }
    }

    private async Task TerminateSessionAsync(
        UserSession userSession,
        SessionTerminationReason reason)
    {
        if (!userSession.IsActive)
        {
            return;
        }

        userSession.TerminateSession(reason);
        await _unitWork.SaveChangesAsync();
        await _sessionCacheService.RemoveSessionAsync(userSession.ClientSessionToken);
    }

    private async Task<UserSession> CreateAnonymousSessionAsync(
        IUserSession? currentSession)
    {
        var sessionToken = HashHelper.CreateSha256Hash(Guid.NewGuid());
        var anonymouseUser_Id = AnonymousConstants.AnonymousUser_Id;
        var currentSessionId = currentSession?.Id ?? AnonymousConstants.AnonymousSystemSession_Id;
        var headerInfo = _userSessionService.GetRequestHeaderInfo();

        var userSession = new UserSession(
              applicationName: headerInfo.ApplicationName,
              clientSessionToken: sessionToken,
              ipAddress: headerInfo.IpAddress,
              userAgent: headerInfo.UserAgent,
              language: Language.Default,
              authenticationType: AuthenticationType.Anonymous,
              user_Id: anonymouseUser_Id,
              authToken: null,
              tenant_Id: null
         );

        _unitWork.Add(userSession);

        var result = await _unitWork.SaveChangesAsync();
        if (!result.IsSuccess)
        {
            throw new InvalidOperationException("Error while creating anonymous session.", result.Exception);
        }
        await _sessionCacheService.AddSessionAsync(userSession);
        return userSession;
    }

    public ValueTask DisposeAsync()
    {
        return _unitWork.DisposeAsync();
    }
}
