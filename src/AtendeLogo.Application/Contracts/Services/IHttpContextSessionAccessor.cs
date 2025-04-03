using AtendeLogo.Common.Infos;
using AtendeLogo.Shared.Models.Security;

namespace AtendeLogo.Application.Contracts.Services;

public interface IHttpContextSessionAccessor : IApplicationService
{
    ClientRequestHeaderInfo RequestHeaderInfo { get; }
    string? AuthorizationToken { get; set; }
    IUserSession? UserSession { get; set; }
    IEndpointService? EndpointInstance { get; set; }
    UserSessionClaims? UserSessionClaims { get; set; }
}

