
namespace AtendeLogo.TestCommon.Mocks;

public class UserSessionVerificationServiceMock: IUserSessionVerificationService
{
    private readonly IUserSessionAccessor _userSessionAccessor;
    public UserSessionVerificationServiceMock(IUserSessionAccessor userSessionAccessor)
    {
        _userSessionAccessor = userSessionAccessor;
    }

    public Task<IUserSession> VerifyAsync()
    {
        var currentUserSession = _userSessionAccessor.GetCurrentSession();
        return Task.FromResult(currentUserSession);
    }
}

