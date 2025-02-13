namespace AtendeLogo.Application.Contracts.Services;

public interface ISessionCacheService
{
    Task<bool> ExistsAsync(string clientSessionToken);

    Task<UserSession?> GetSessionAsync(
        string clientSessionToken,
        CancellationToken cancellationToken = default);

    Task AddSessionAsync(
        UserSession session,
        CancellationToken cancellationToken = default);

    Task RemoveSessionAsync(
        string clientSessionToken,
        CancellationToken cancellationToken = default);
}
