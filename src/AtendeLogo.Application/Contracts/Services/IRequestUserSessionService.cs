using AtendeLogo.Common.Infos;

namespace AtendeLogo.Application.Contracts.Services;

public interface IRequestUserSessionService : IApplicationService
{
    void AddClientSessionCookie(string session);
    string? GetClientSessionToken();
    IUserSession GetCurrentSession();
    RequestHeaderInfo GetRequestHeaderInfo();
}
