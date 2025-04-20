using AtendeLogo.Common.Enums;
using AtendeLogo.Common.Infos;
using AtendeLogo.Shared.Models.Security;

namespace AtendeLogo.Application.Abstractions.Services;

public interface IHttpContextSessionAccessor : IApplicationService
{
    string RequestUrl { get; }
    Guid? UserSession_Id { get; }
    Culture Culture { get; }
    Language Language { get; }
    ClientRequestHeaderInfo RequestHeaderInfo { get; }
    string? AuthorizationToken { get; set; }
    IUserSession? UserSession { get; set; }
    IEndpointService? EndpointInstance { get; set; }
    UserSessionClaims? UserSessionClaims { get; set; }
}

