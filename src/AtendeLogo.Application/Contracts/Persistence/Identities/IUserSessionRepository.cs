
namespace AtendeLogo.Application.Contracts.Persistence.Identities;

public interface IUserSessionRepository : IRepositoryBase<UserSession>
{
    Task<UserSession?> GetByClientTokenAsync(
        string sessionToken, 
        CancellationToken cancellationToken = default);
}
