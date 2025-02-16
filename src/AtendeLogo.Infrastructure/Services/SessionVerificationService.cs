using System.Diagnostics;
using AtendeLogo.Application.Contracts.Persistence.Identity;
using AtendeLogo.Common;
using AtendeLogo.Common.Infos;

namespace AtendeLogo.Infrastructure.Services;

public class SessionVerificationService : IAsyncDisposable
{
    private readonly RequestHeaderInfo _headerInfo;
    private readonly ISessionCacheService _sessionCacheService;
    private readonly IIdentityUnitOfWork _unitWork;
    private readonly string? _clientSessionToken;

    private IUserSessionRepository UserSessionRepository
        => _unitWork.UserSessionRepository;

    public SessionVerificationService(
        ISessionCacheService cacheSessionService,
        IIdentityUnitOfWork unitWork,
        RequestHeaderInfo headerInfo,
        string? clientSessionToken)
    {
        _sessionCacheService = cacheSessionService;
        _unitWork = unitWork;
        _headerInfo = headerInfo;
        _clientSessionToken = clientSessionToken;
    }

    public async Task<UserSession> RetrieveSessionAsync()
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

    private async Task<UserSession?> GetUserSessionAsync()
    {
        if (string.IsNullOrWhiteSpace(_clientSessionToken))
            return null;

        var cachedSession = await GetSessionFromCacheAsync(_clientSessionToken);
        if (cachedSession is not null)
        {
            return cachedSession;
        }
        return await UserSessionRepository.GetByClientTokenAsync(_clientSessionToken);
    }

    private async Task<UserSession?> GetSessionFromCacheAsync(string clientSessionToken)
    {
        var cachedSession = await _sessionCacheService.GetSessionAsync(clientSessionToken);
        if (cachedSession?.IsUpdatePending() == true)
        {
            return await UserSessionRepository.TryRefreshAsync(cachedSession);
        }
        return cachedSession;
    }

    private async Task ValidateSessionAsync(
        UserSession userSession)
    {
        if (!userSession.IsActive)
            return;

        if (userSession.IsUpdatePending() || Debugger.IsAttached)
        {
            userSession.UpdateLastActivity();

            _unitWork.Update(userSession);

            var result = await _unitWork.SaveChangesAsync();
            if (result.Error is DomainEventError)
            {
                await TerminateSessionAsync(userSession, SessionTerminationReason.DomainEventError);
                return;
            }
            await _sessionCacheService.AddSessionAsync(userSession);
        }

        if (!string.Equals(userSession.IpAddress, _headerInfo.IpAddress, StringComparison.OrdinalIgnoreCase))
        {
            await TerminateSessionAsync(userSession, SessionTerminationReason.IpAddressChanged);
            return;
        }

        if (!string.Equals(userSession.UserAgent, _headerInfo.UserAgent, StringComparison.OrdinalIgnoreCase))
        {
            await TerminateSessionAsync(userSession, SessionTerminationReason.UserAgentChanged);
            return;
        }
    }

    private async Task TerminateSessionAsync(
        UserSession userSession,
        SessionTerminationReason reason)
    {
        userSession.TerminateSession(reason);

        await _unitWork.SaveChangesAsync();
        await _sessionCacheService.RemoveSessionAsync(userSession.ClientSessionToken);
    }

    private async Task<UserSession> CreateAnonymousSessionAsync(
        UserSession? currentSession)
    {
        var sessionToken = HashHelper.CreateSha256Hash(Guid.NewGuid());
        var anonymouseUser_Id = AnonymousConstants.AnonymousUser_Id;
        var currentSessionId = currentSession?.Id ?? AnonymousConstants.AnonymousSystemSession_Id;

        var userSession = new UserSession(
              applicationName: _headerInfo.ApplicationName,
              clientSessionToken: sessionToken,
              ipAddress: _headerInfo.IpAddress,
              userAgent: _headerInfo.UserAgent,
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
