using AtendeLogo.Application.Contracts.Persistence.Identities;

namespace AtendeLogo.Application.UnitTests.Persistence.Identity;

public class SystemUserRepositoryTests: IClassFixture<AnonymousServiceProviderMock>
{
    private readonly IServiceProvider _serviceProvider;

    public SystemUserRepositoryTests(
        AnonymousServiceProviderMock serviceProvider,
        ITestOutputHelper testOutput)
    {
        serviceProvider.AddTestOutput(testOutput);

        _serviceProvider = serviceProvider;
    }

    [Fact]
    public async Task GetAllUsers_ShouldReturnNonEmptyList()
    {
        // Arrange
        var systemUserRepository = _serviceProvider.GetRequiredService<ISystemUserRepository>();

        // Act
        var users = await systemUserRepository.GetAllAsync();

        // Assert
        systemUserRepository.Should()
            .NotBeNull();

        users.Should()
            .NotBeNull()
            .And
            .NotBeEmpty();
    }

    [Fact]
    public async Task GetById_ShouldReturnAnonymousUser()
    {
        // Arrange
        var systemUserRepository = _serviceProvider.GetRequiredService<ISystemUserRepository>();
        var anonymousId = AnonymousIdentityConstants.User_Id;
      
        // Act
        var anonymousUser = await systemUserRepository.GetByIdAsync(anonymousId);
       
        // Assert
        anonymousUser.Should()
            .NotBeNull();

        anonymousUser!.Id.Should()
            .Be(anonymousId);
    }
}

