using AtendeLogo.Shared.Models.Security;

namespace AtendeLogo.Application.Abstractions.Services;

public interface IUserSessionTokenHandler : IApplicationService
{
    string WriteToken(UserSessionClaims userSessionClaims, bool isPersistent);

    UserSessionClaims? ReadToken(string token);
}
