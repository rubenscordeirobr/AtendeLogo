using AtendeLogo.Shared.Models.Identities;

namespace AtendeLogo.Application.Contracts.Services;

public interface IUserSessionCacheService : IApplicationService
{
    Task<bool> ExistsAsync(Guid session_Id, CancellationToken cancellationToken = default);

    Task<CachedUserSession?> GetSessionAsync(
        Guid session_Id,
        CancellationToken cancellationToken = default);

    Task AddSessionAsync(
        IUserSession session,
        CancellationToken cancellationToken = default);

    Task RemoveSessionAsync(
        Guid session_Id,
        CancellationToken cancellationToken = default);
}
