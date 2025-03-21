using AtendeLogo.Common.Infos;

namespace AtendeLogo.Application.Contracts.Services;

public interface IUserSessionAccessor : IApplicationService
{
    void AddClientSessionCookie(string clientSessionToken);
    void RemoveClientSessionCookie(string clientSessionToken);
    string? GetClientSessionToken();
    IUserSession GetCurrentSession();

    ClientRequestHeaderInfo GetClientRequestHeaderInfo();
    IEndpointService? GetCurrentEndpointInstance();
}
