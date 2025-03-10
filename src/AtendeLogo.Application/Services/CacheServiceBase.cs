﻿using System.Text.Json;
using System.Text.Json.Serialization;
using AtendeLogo.Application.Common.JsonConverters;
using Microsoft.Extensions.Logging;

namespace AtendeLogo.Application.Services;

public abstract class CacheServiceBase
{
    private readonly ICacheRepository _repository;
    private readonly ILogger _logger;
    private readonly TimeSpan _defaultExpiration;
    protected abstract string PrefixCacheName { get; }

    protected virtual JsonSerializerOptions JsonOptions { get; }
        = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            IncludeFields = true,
            IgnoreReadOnlyProperties = false,
            WriteIndented = true,
            IgnoreReadOnlyFields= true,
        };

    protected CacheServiceBase(
        ICacheRepository _repository,
        ILogger logger,
        TimeSpan expiration)
    {
        this._repository = _repository;
        _logger = logger;
        _defaultExpiration = expiration;
    }

    protected async Task<bool> ExistsInCacheAsync(Guid key)
    {
        var cacheKey = BuildCacheKey(key);
        return await _repository.KeyExistsAsync(cacheKey);
    }

    protected async Task<bool> ExistsInCacheAsync(string key)
    {
        var cacheKey = BuildCacheKey(key);
        return await _repository.KeyExistsAsync(cacheKey);
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
        await AddToCacheAsyncInternal(cacheKey, value, expiration);
    }

    protected async Task AddToCacheAsync<T>(Guid key, T value, TimeSpan? expiration = null)
    {
        var cacheKey = BuildCacheKey(key);
        await AddToCacheAsyncInternal(cacheKey, value, expiration);
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
        var cachedValue = await _repository.StringGetAsync(cachedKey);
        if (cachedValue is null || cancellationToken.IsCancellationRequested) 
        {
            return default;
        }

        try
        {
            var options = GetJsonOptions<T>();
            return JsonSerializer.Deserialize<T>(cachedValue, options);
        }
        catch (JsonException)
        {
            _logger.LogError("Failed to deserialize cached value for key {Key} to {Type}", cachedKey, typeof(T).Name);
            return default;
        }
    }
     
    private async Task AddToCacheAsyncInternal<T>(string cacheKey, T value, TimeSpan? expiration = null)
    {
        var serializedValue = JsonSerializer.Serialize(value);
        await _repository.StringSetAsync(cacheKey, serializedValue, expiration ?? _defaultExpiration);
    }

    private async Task RemoveFromCacheAsyncInternal(string cacheKey)
    {
        await _repository.KeyDeleteAsync(cacheKey);
    }

    private string BuildCacheKey(
         string id)
         => BuildCacheKey(HashHelper.CreateMd5GuidHash(id));

    private string BuildCacheKey(Guid id)
        => $"{PrefixCacheName}:{id}";

    private JsonSerializerOptions? GetJsonOptions<T>()
    {
        if (typeof(T).IsSubclassOf(typeof(EntityBase)))
        {
            var converterType = typeof(EntityJsonConverter<>).MakeGenericType(typeof(T));
            var converterInstance = Activator.CreateInstance(converterType) as JsonConverter<T>;

            Guard.NotNull(converterInstance, nameof(converterInstance));

            return new JsonSerializerOptions(JsonOptions) 
            {
                Converters = { converterInstance }
            };
        }
        return JsonOptions;
    }
}
