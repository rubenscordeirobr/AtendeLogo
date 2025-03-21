namespace AtendeLogo.TestCommon.Mocks.Infrastructure;

public sealed class SecureConfigurationMock: ISecureConfiguration
{
    public string GetPasswordSalt()
    {
        return "SYSTEM";
    }
}
