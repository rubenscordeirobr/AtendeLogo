namespace AtendeLogo.RuntimeServices.Contracts;

internal interface IUserSessionVerificationServiceTest
{
    Task<UserSession> CreateAnonymousSessionAsync();
}
