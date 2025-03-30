namespace AtendeLogo.Application.Contracts.Security;
public interface ISecureConfiguration
{
    string GetAuthenticationKey();
    string GetJwtAudience();
    string GetJwtIssuer();
    string GetPasswordSalt();
}
