using Microsoft.Extensions.DependencyInjection;

namespace AtendeLogo.ArchitectureTests.TestSupport;

public class ClientGatewayServiceProvider : IServiceProvider
{
    private readonly IServiceProvider _serviceProvider;
    public IServiceCollection Services { get; }

    public ClientGatewayServiceProvider()
    {
        Services = new ClientGatewayServiceCollection();
        _serviceProvider = Services.BuildServiceProvider();
    }

    public object? GetService(Type serviceType)
    {
        return _serviceProvider.GetService(serviceType);
    }
}
