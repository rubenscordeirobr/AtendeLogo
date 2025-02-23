
using System.Collections.Concurrent;

namespace AtendeLogo.Application.UnitTests.Mocks;

public class CacheRepositoryMock : ICacheRepository
{
    private readonly ConcurrentDictionary<string, string> _cache = new();

    public Task<bool> KeyExistsAsync(string cacheKey)
    {
        return Task.FromResult(_cache.ContainsKey(cacheKey));
    }

    public Task<string?> StringGetAsync(string cachedKey)
    {
        return Task.FromResult(_cache.TryGetValue(cachedKey, out var value) ? value : null);
    }

    public Task KeyDeleteAsync(string cacheKey)
    {
        _cache.TryRemove(cacheKey, out _);
        return Task.CompletedTask;
    }

    public Task StringSetAsync(
        string cacheKey, 
        string value,
        TimeSpan timeSpan)
    {
        _cache.AddOrUpdate(cacheKey, value, (_, _) => value);
        return Task.CompletedTask;
    }
}

