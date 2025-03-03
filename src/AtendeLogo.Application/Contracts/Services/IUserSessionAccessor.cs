using AtendeLogo.Common.Infos;

namespace AtendeLogo.Application.Contracts.Services;

public interface IUserSessionAccessor : IApplicationService
{
    void AddClientSessionCookie(string session);
    string? GetClientSessionToken();
    IUserSession GetCurrentSession();
    RequestHeaderInfo GetRequestHeaderInfo();
}
