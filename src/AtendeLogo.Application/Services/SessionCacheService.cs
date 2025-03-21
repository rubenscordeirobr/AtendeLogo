using Microsoft.Extensions.Logging;

namespace AtendeLogo.Application.Services;

public class SessionCacheService : CacheServiceBase, ISessionCacheService
{
    protected override string PrefixCacheName => "user-session";

    public SessionCacheService(
        ICacheRepository cacheRepository,
        ILogger<SessionCacheService> logger)
        : base(cacheRepository, logger, TimeSpan.FromHours(1))
    {
    }

    public Task<bool> ExistsAsync(string clientSessionToken)
    {
        return ExistsInCacheAsync(clientSessionToken);
    }

    public async Task<UserSession?> GetSessionAsync(
        string clientSessionToken, 
        CancellationToken cancellationToken = default)
    {
        return await GetFromCacheAsync<UserSession>(clientSessionToken, cancellationToken);
    }

    public async Task AddSessionAsync(
        UserSession session, 
        CancellationToken cancellationToken = default)
    {
        Guard.NotNull(session);

        if(session.Id == Guid.Empty)
        {
            throw new InvalidOperationException("Cannot add session with empty Id");
        }

        await AddToCacheAsync(session.ClientSessionToken, session);
    }

    public Task RemoveSessionAsync(
        string clientSessionToken, 
        CancellationToken cancellationToken = default)
    {
        return RemoveFromCacheAsync(clientSessionToken);
    }
}
