namespace AtendeLogo.TestCommon.Mocks.Infrastructure;

internal class SecureConfigurationMock: ISecureConfiguration
{
    public string GetPasswordSalt()
    {
        return "SYSTEM";
    }
}
