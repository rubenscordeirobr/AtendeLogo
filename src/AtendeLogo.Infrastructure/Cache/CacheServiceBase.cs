using System.Text.Json;
using System.Text.Json.Serialization;
using AtendeLogo.Common;
using AtendeLogo.Domain.Primitives;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace AtendeLogo.Infrastructure.Cache;

public abstract class CacheServiceBase
{
    private readonly IDatabase _cache;
    private readonly ILogger<CacheServiceBase> _logger;
    private readonly TimeSpan _defaultExpiration;
    protected abstract string PrefixChacheName { get; }

    protected virtual JsonSerializerOptions JsonOptiops { get; }
        = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            IncludeFields = true,
            IgnoreReadOnlyProperties = false,
            WriteIndented = true,
            IgnoreReadOnlyFields= true,
        };

    protected CacheServiceBase(
        IConnectionMultiplexer redisConnection,
        ILogger<CacheServiceBase> logger,
        TimeSpan expiration)
    {
        _cache = redisConnection.GetDatabase();
        _logger = logger;
        _defaultExpiration = expiration;
    }

    protected async Task<bool> ExistsInCacheAsync(Guid key)
    {
        var cacheKey = BuildCacheKey(key);
        return await _cache.KeyExistsAsync(cacheKey);
    }

    protected async Task<bool> ExistsInCacheAsync(string key)
    {
        var cacheKey = BuildCacheKey(key);
        return await _cache.KeyExistsAsync(cacheKey);
    }

    protected async Task<T?> GetFromCacheAsync<T>(Guid key, CancellationToken cancellationToken = default)
    {
        var cacheKey = BuildCacheKey(key);
        return await GetAsyncInternal<T>(cacheKey, cancellationToken);
    }

    protected async Task<T?> GetFromCacheAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var cacheKey = BuildCacheKey(key);
        return await GetAsyncInternal<T>(cacheKey, cancellationToken);
    }

    protected async Task AddToCacheAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var cacheKey = BuildCacheKey(key);
        await AddToCacheAsynccInternal(cacheKey, value, expiration);
    }

    protected async Task AddToCacheAsync<T>(Guid key, T value, TimeSpan? expiration = null)
    {
        var cacheKey = BuildCacheKey(key);
        await AddToCacheAsynccInternal(cacheKey, value, expiration);
    }

    protected async Task RemoveFromCacheAsync(Guid key)
    {
        var cacheKey = BuildCacheKey(key);
        await RemoveFromCacheAsyncInternal(cacheKey);
    }

    protected async Task RemoveFromCacheAsync(string key)
    {
        var cacheKey = BuildCacheKey(key);
        await RemoveFromCacheAsyncInternal(cacheKey);
    }

    private async Task<T?> GetAsyncInternal<T>(string cachedKey, CancellationToken cancellationToken = default)
    {
        var cachedValue = await _cache.StringGetAsync(cachedKey);
        if (!cachedValue.HasValue || cancellationToken.IsCancellationRequested)
        {
            return default;
        }

        try
        {
            var options = GetJsonOptios<T>();
            return JsonSerializer.Deserialize<T>(cachedValue.ToString(), options);
        }
        catch (JsonException)
        {
            _logger.LogError("Failed to deserialize cached value for key {Key} to {Type}", cachedKey, typeof(T).Name);
            return default;
        }
    }
     
    private async Task AddToCacheAsynccInternal<T>(string cacheKey, T value, TimeSpan? expiration = null)
    {
        var serializedValue = JsonSerializer.Serialize(value);
        await _cache.StringSetAsync(cacheKey, serializedValue, expiration ?? _defaultExpiration);
    }

    private async Task RemoveFromCacheAsyncInternal(string cacheKey)
    {
        await _cache.KeyDeleteAsync(cacheKey);
    }

    private string BuildCacheKey(
         string id)
         => BuildCacheKey(HashHelper.CreateMd5GuidHash(id));

    private string BuildCacheKey(Guid id)
        => $"{PrefixChacheName}:{id}";

    private JsonSerializerOptions? GetJsonOptios<T>()
    {
        if (typeof(T).IsSubclassOf(typeof(EntityBase)))
        {
            var converterType = typeof(EntityJsonConverter<>).MakeGenericType(typeof(T));
            var converterInstance = Activator.CreateInstance(converterType) as JsonConverter<T>;

            Guard.NotNull(converterInstance, nameof(converterInstance));

            return new JsonSerializerOptions(JsonOptiops) 
            {
                Converters = { converterInstance }
            };
        }
        return JsonOptiops;
    }
}
