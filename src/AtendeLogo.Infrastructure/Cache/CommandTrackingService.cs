using AtendeLogo.Common;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace AtendeLogo.Infrastructure.Cache;

public class CommandTrackingService : CacheServiceBase, ICommandTrackingService
{
    protected override string PrefixChacheName
        => "request-tracker";

    public CommandTrackingService(
        IConnectionMultiplexer redisConnection,
        ILogger<CommandTrackingService> logger)
        : base(redisConnection, logger, TimeSpan.FromMinutes(5))
    {
    }

    public async Task<bool> ExistsAsync(Guid clientRequestId, CancellationToken cancellationToken)
    {
        return await ExistsInCacheAsync(clientRequestId);
    }

    public async Task<Result<T>?> TryGetResultAsync<T>(Guid clientRequestId, CancellationToken cancellationToken)
        where T : notnull
    {
        var value = await GetFromCacheAsync<T>(clientRequestId, cancellationToken);
        if (value is null)
        {
            return null;
        }
        return Result.Success(value);
    }

    public async Task TrackAsync<T>(Guid clientRequestId, Result<T> result) where T : notnull
    {
        if (!result.IsSuccess)
        {
            throw new InvalidOperationException("Only successful results can be tracked.");
        }
        await AddToCacheAsync(clientRequestId, result.Value);
    }
   
}
