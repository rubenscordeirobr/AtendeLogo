namespace AtendeLogo.Domain.Entities.Activities;

public class SuccessfulAuthenticationActivity : AuthenticationActivity
{
    public AuthenticationType AuthenticationType { get; private set; }

    public SuccessfulAuthenticationActivity(AuthenticationType authenticationType, string ipAddress)
        : base(ipAddress)
    {
        ActivityType = ActivityType.SuccessfulAuthentication;
    }
}
