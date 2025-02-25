using AtendeLogo.Application.Contracts.Persistence.Identity;
using AtendeLogo.UseCases.Identities.Users.TenantUsers.Queries;

namespace AtendeLogo.UseCases.UnitTests.Identities.Users.TenantUsers;

public class GetTenantUserByEmailQueryHandlerTests : IClassFixture<TenantUserServiceProviderMock>
{
    private readonly IServiceProvider _serviceProvider;

    public GetTenantUserByEmailQueryHandlerTests(TenantUserServiceProviderMock serviceProvider)
    {
        this._serviceProvider = serviceProvider;
    }

    [Fact]
    public void Handle_ShouldBe_GetTenantUserByEmailQueryHandler()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>() as IRequestMediatorTest;
        var query = new GetTenantUserByEmailQuery("tenantuser@example.com");

        // Act
        var handlerType = mediator!.GetRequestHandler(query);

        // Assert
        handlerType.Should().BeOfType<GetTenantUserByEmailQueryHandler>();
    }

    [Fact]
    public async Task HandleAsync_ReturnSuccess()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>();
        var tenantUserRepository = _serviceProvider.GetRequiredService<ITenantUserRepository>();
        var tenantUser = await tenantUserRepository.GetByEmailAsync(SystemTenantConstants.TenantSystemEmail);

        Guard.NotNull(tenantUser);

        var query = new GetTenantUserByEmailQuery(tenantUser.Email);

        // Act
        var result = await mediator.GetSingleAsync(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        var tenantUserResponse = result.Value
            .Should()
            .BeOfType<TenantUserResponse>()
            .Subject;

        tenantUserResponse.Email.Should().Be(tenantUser.Email);
    }

    [Fact]
    public async Task HandleAsync_ReturnFailure()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>();
        var query = new GetTenantUserByEmailQuery("nonexistentuser@example.com");

        // Act
        var result = await mediator.GetSingleAsync(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }
}

