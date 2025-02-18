
namespace AtendeLogo.Application.UnitTests.Mocks.Infrastructure;

internal class SessionCacheServiceMock : ISessionCacheService
{
    public Task<bool> ExistsAsync(string clientSessionToken)
    {
        return Task.FromResult(false);
    }

    public Task<UserSession?> GetSessionAsync(
        string clientSessionToken,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<UserSession?>(null);
    }

    public Task AddSessionAsync(
        UserSession session,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(true);
    }

    public Task RemoveSessionAsync(
        string clientSessionToken, 
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(false);
    }
}
