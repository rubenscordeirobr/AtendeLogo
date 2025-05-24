using AtendeLogo.UseCases.Identities.UserSessions;

namespace AtendeLogo.UseCases.Abstractions.Identities;

public interface IUserSessionService
{
    Task ChangeLanguageAsync(
        ChangeUserSessionLanguageCommand command, 
        CancellationToken cancellationToken = default);
}
