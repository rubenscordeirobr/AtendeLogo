using AtendeLogo.Common;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace AtendeLogo.Infrastructure.Cache;

public class SessionCacheService : CacheServiceBase, ISessionCacheService
{
    protected override string PrefixChacheName => "user-session";

    public SessionCacheService(
        IConnectionMultiplexer redisConnection,
        ILogger<CommandTrackingService> logger)
        : base(redisConnection, logger, TimeSpan.FromHours(1))
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

        if(session.Id == default)
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
