
namespace AtendeLogo.Application.Contracts.Persistence.Identities;

public interface IUserSessionRepository : IRepositoryBase<UserSession>
{
    
    Task<UserSession?> GetByClientTokenAsync(string sessionToken);

    //Task CreateAsync(UserSession userSession);
    //Task UpdateAsync(UserSession userSession);
}
