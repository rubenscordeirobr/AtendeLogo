namespace AtendeLogo.Application.Contracts.Services;

public interface IUserSessionManager : IApplicationService
{
    Guid? UserSession_Id { get; }
    Task SetSessionAsync(IUserSession userSession, IUser user);
    Task<IUserSession?> GetSessionAsync();
    Task RemoveSessionAsync();
    Task RemoveSessionAsync(Guid userSession_Id);
}

