using AtendeLogo.Application.Contracts.Persistence.Identities;
using AtendeLogo.Common.Infos;
using AtendeLogo.Domain.Entities.Identities.Factories;
using AtendeLogo.Shared.Models.Identities;

namespace AtendeLogo.Application.Services;

public class UserSessionVerificationService : IUserSessionVerificationService, IAsyncDisposable
{
    private readonly ISessionCacheService _sessionCacheService;
    private readonly IIdentityUnitOfWork _unitWork;
    private readonly IUserSessionAccessor _userSessionAccessor;

    private IUserSessionRepository UserSessionRepository
        => _unitWork.UserSessions;

    public UserSessionVerificationService(
        ISessionCacheService cacheSessionService,
        IUserSessionAccessor userSessionAccessor,
        IIdentityUnitOfWork unitWork)
    {
        _sessionCacheService = cacheSessionService;
        _userSessionAccessor = userSessionAccessor;
        _unitWork = unitWork;
    }

    public async Task<IUserSession> VerifyAsync()
    {
        var session = await RetrieveSessionAsync();
        _userSessionAccessor.AddClientSessionCookie(session.ClientSessionToken);
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
        var clientSessionToken = _userSessionAccessor.GetClientSessionToken();
        if (string.IsNullOrWhiteSpace(clientSessionToken))
            return null;

        var cachedSession = await GetSessionFromCacheAsync(clientSessionToken);
        if (cachedSession is not null)
        {
            return cachedSession;
        }
        return await UserSessionRepository.GetByClientTokenAsync(
            clientSessionToken);
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
        {
            return;
        }

        if (userSession is AnonymousUserSession)
        {
            return;
        }

        if (userSession is not UserSession userSessionEntity)
        {
            throw new InvalidOperationException("Invalid session type.");
        }

        await ValidateSessionEntityAsync(userSessionEntity);
    }

    private async Task ValidateSessionEntityAsync(UserSession userSession)
    {
        var headerInfo = _userSessionAccessor.GetClientRequestHeaderInfo();
        var terminationReason = GetSessionTerminationReason(userSession, headerInfo);
        if (terminationReason.HasValue)
        {
            await TerminateSessionAsync(userSession, terminationReason.Value);
            return;
        }

        if (!userSession.IsUpdatePending())
        {
            return;
        }

        await ProcessSessionUpdateAsync(userSession);
    }

    private SessionTerminationReason? GetSessionTerminationReason(
        UserSession userSession,
        ClientRequestHeaderInfo headerInfo)
    {
        if (userSession.IsExpired())
        {
            return SessionTerminationReason.SessionExpired;
        }

        if (!string.Equals(userSession.IpAddress, headerInfo.IpAddress, StringComparison.OrdinalIgnoreCase))
        {
            return SessionTerminationReason.IpAddressChanged;
        }

        if (!string.Equals(userSession.UserAgent, headerInfo.UserAgent, StringComparison.OrdinalIgnoreCase))
        {
            return SessionTerminationReason.UserAgentChanged;
        }
        return null;
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

    private async Task ProcessSessionUpdateAsync(UserSession userSession)
    {
        userSession.UpdateLastActivity();

        _unitWork.Update(userSession);

        var result = await _unitWork.SaveChangesAsync(silent: true);
        if (!result.IsSuccess)
        {
            var terminationReason = result.Error is DomainEventError
                ? SessionTerminationReason.DomainEventError
                : throw result.Exception;

            await TerminateSessionAsync(userSession, terminationReason);
            return;
        }

        await _sessionCacheService.AddSessionAsync(userSession);
    }

    private async Task<UserSession> CreateAnonymousSessionAsync(
        IUserSession? currentSession)
    {
        var currentSessionId = currentSession?.Id ?? AnonymousIdentityConstants.Session_Id;
        var headerInfo = _userSessionAccessor.GetClientRequestHeaderInfo();
        var anonymousUser = AnonymousIdentityConstants.AnonymousUser;
        var userSession = UserSessionFactory.Create(
              user: anonymousUser,
              clientHeaderInfo: headerInfo,
              authenticationType: AuthenticationType.Anonymous,
              rememberMe: true,
              tenant_id: null);

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
