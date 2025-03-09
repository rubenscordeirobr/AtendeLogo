using AtendeLogo.Application.Contracts.Security;

namespace AtendeLogo.Application.UnitTests.Mocks.Infrastructure;

internal class SecureConfigurationMock: ISecureConfiguration
{
    public string GetPasswordSalt()
    {
        return "SYSTEM";
    }
}
