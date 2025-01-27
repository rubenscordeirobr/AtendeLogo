namespace AtendeLogo.Domain.Entities.Activities;

public abstract class AuthenticationActivity : ActivityBase
{
    public string IPAddress { get; private set; }
    
    protected AuthenticationActivity(string ipAddress)
    {
        ArgumentException.ThrowIfNullOrEmpty(ipAddress);
 
        IPAddress = ipAddress;
    }
}
