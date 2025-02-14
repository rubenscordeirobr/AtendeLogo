namespace AtendeLogo.Application.Contracts.Security;
public interface ISecureConfiguration
{
    string GetPasswordSalt();
}
