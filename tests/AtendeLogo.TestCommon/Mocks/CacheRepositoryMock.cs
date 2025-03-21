using System.Collections.Concurrent;
using AtendeLogo.Common.Helpers;
using AtendeLogo.Common.Utils;

namespace AtendeLogo.TestCommon.Mocks;

public class CacheRepositoryMock : ICacheRepository
{
    private const string UserSessionPrefix = "user-session:";

    private readonly ConcurrentDictionary<string, string> _cache = new();

    private readonly IUserSessionAccessor? _userSessionAccessor;

    [ActivatorUtilitiesConstructor]
    public CacheRepositoryMock(IUserSessionAccessor userSessionAccessor)
    {
        _userSessionAccessor = userSessionAccessor;
    }

    public CacheRepositoryMock()
    {
        
    }

    public Task<bool> KeyExistsAsync(string cacheKey)
    {
        return Task.FromResult(_cache.ContainsKey(cacheKey));
    }

    public Task<string?> StringGetAsync(string cachedKey)
    {
        var userSession = TryGetCurrentSessionFromAccessor(cachedKey);
        if(userSession is not null)
        {
            var json = JsonUtils.Serialize(userSession);
            return Task.FromResult(json)!;
        }
        return Task.FromResult(_cache.TryGetValue(cachedKey, out var value) ? value : null);
    }

    private IUserSession? TryGetCurrentSessionFromAccessor(string cachedKey)
    {
  
        if (_userSessionAccessor is null || !cachedKey.StartsWith(UserSessionPrefix))
            return null;

        var cacheIdText = cachedKey[UserSessionPrefix.Length..];

        if (!Guid.TryParse(cacheIdText, out var cacheId))
            return null;

        var currentSession = _userSessionAccessor.GetCurrentSession();
        if (currentSession is null)
            return null;

        var clientSessionChacheId = HashHelper.CreateMd5GuidHash(currentSession.ClientSessionToken);

        return clientSessionChacheId == cacheId
            ? currentSession
            : null;
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

