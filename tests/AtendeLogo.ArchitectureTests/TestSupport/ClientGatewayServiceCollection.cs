using AtendeLogo.UseCases;
using AtendeLogo.ClientGateway;
using Microsoft.Extensions.DependencyInjection;

namespace AtendeLogo.ArchitectureTests.TestSupport ;

public class ClientGatewayServiceCollection : ServiceCollection
{
    public ClientGatewayServiceCollection()
    {
        this.AddUserCasesSharedServices()
            .AddClientGatewayServices();

        this.AddSingleton<ITestOutputHelper, TestOutputProxy>();
    }
}
