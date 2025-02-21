using StackExchange.Redis;

namespace AtendeLogo.Infrastructure.Cache;

public class CacheRepository : ICacheRepository
{
    private IDatabase _database;

    public CacheRepository(IConnectionMultiplexer connection)
    {
        _database = connection.GetDatabase() ;
    }

    public Task<bool> KeyExistsAsync(string cacheKey)
    {
        return _database.KeyExistsAsync(cacheKey);
    }

    public async Task<string?> StringGetAsync(string cachedKey )
    {
        var cachedValue = await _database.StringGetAsync(cachedKey);
        if (!cachedValue.HasValue)
        {
            return null;
        }
        return cachedValue.ToString();
    }
     
    public Task KeyDeleteAsync(string cacheKey)
    {
        return _database.KeyDeleteAsync(cacheKey);
    }

    public Task StringSetAsync(string cacheKey, string value, TimeSpan timeSpan)
    {
        return _database.StringSetAsync(cacheKey, value, timeSpan);
    }
}
