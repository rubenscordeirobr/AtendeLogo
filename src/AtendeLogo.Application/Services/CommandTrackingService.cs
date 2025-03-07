using Microsoft.Extensions.Logging;

namespace AtendeLogo.Infrastructure.Cache;

public class CommandTrackingService : CacheServiceBase, ICommandTrackingService
{
    protected override string PrefixCacheName 
        => "request-tracker";

    public CommandTrackingService(
        ICacheRepository cacheRepository,
        ILogger<CommandTrackingService> logger)
        : base(cacheRepository, logger, TimeSpan.FromMinutes(5))
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
