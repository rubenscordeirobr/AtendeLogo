
namespace AtendeLogo.Application.Contracts.Services;

public interface IUserSessionVerificationService: IApplicationService
{
    Task<IUserSession> VerifyAsync();
}
