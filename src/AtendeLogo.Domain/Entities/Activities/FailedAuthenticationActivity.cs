namespace AtendeLogo.Domain.Entities.Activities;

public class FailedAuthenticationActivity : AuthenticationActivity
{
    public string? PasswordFailed { get; private set; }
    public string? UserAgent { get; private set; }

    public FailedAuthenticationActivity(string ipAddress, string? userAgent, string? passwordFailed)
        : base(ipAddress)
    {
        PasswordFailed = passwordFailed;
        UserAgent = userAgent;
    }
}
