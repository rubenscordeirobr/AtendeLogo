namespace AtendeLogo.RuntimeServices.Abstractions;

internal interface IUserSessionVerificationServiceTest
{
    Task<UserSession> CreateAnonymousSessionAsync();
}
