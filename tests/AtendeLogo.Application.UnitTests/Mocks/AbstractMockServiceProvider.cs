using AtendeLogo.Application.UnitTests.Mocks.Extensions;
using AtendeLogo.UseCases;

namespace AtendeLogo.Application.UnitTests.Mocks;

public abstract class AbstractMockServiceProvider : IServiceProvider
{
    private IServiceProvider _serviceProvider;
    protected abstract bool IsAnonymous { get; }
    public AbstractMockServiceProvider()
    {
        _serviceProvider = new ServiceCollection()
              .AddApplicationServices()
              .AddLoggerServiceMock()
              .AddInMemoryIdentityDbContext()
              .AddMockInfrastructureServices(IsAnonymous)
              .AddPersistenceServicesMock()
              .AddUserCasesServices()
              .AddUserCasesSharedServices()
              .BuildServiceProvider();
    }

    public object? GetService(Type serviceType)
    {
        return _serviceProvider.GetService(serviceType);
    }
}

public class TenantUserServiceProviderMock : AbstractMockServiceProvider
{
    protected override bool IsAnonymous => false;
}

public class AnonymousServiceProviderMock : AbstractMockServiceProvider
{
    protected override bool IsAnonymous => true;
}
