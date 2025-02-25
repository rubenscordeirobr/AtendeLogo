using AtendeLogo.Application.Contracts.Persistence.Identity;
using AtendeLogo.UseCases.Identities.Users.TenantUsers.Queries;

namespace AtendeLogo.UseCases.UnitTests.Identities.Users.TenantUsers;

public class GetTenantUserByIdQueryHandlerTests : IClassFixture<TenantUserServiceProviderMock>
{
    private readonly IServiceProvider _serviceProvider;

    public GetTenantUserByIdQueryHandlerTests(TenantUserServiceProviderMock serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    [Fact]
    public void HandleType_ShouldBe_GetTenantUserByIdQueryHandler()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>() as IRequestMediatorTest;
        var query = new GetTenantUserByIdQuery(Guid.NewGuid());

        // Act
        var handlerType = mediator!.GetRequestHandler(query);

        // Assert
        handlerType.Should().BeOfType<GetTenantUserByIdQueryHandler>();
    }

    [Fact]
    public async Task HandleAsync_ReturnSuccess()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>();
        var guidAdminUserId = Guid.NewGuid();

        var adminUserRepository = _serviceProvider.GetRequiredService<ITenantUserRepository>();
        var tenantUserTemp = await adminUserRepository.GetByEmailAsync(SystemTenantConstants.TenantSystemEmail);

        Guard.NotNull(tenantUserTemp);

        var query = new GetTenantUserByIdQuery(tenantUserTemp.Id);

        // Act
        var result = await mediator.GetSingleAsync(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        var tenantUser = result.Value;

        tenantUser.Should()
            .BeOfType<TenantUserResponse>();

        tenantUser!.Id.Should().Be(tenantUser.Id);
    }

    [Fact]
    public async Task HandleAsync_ReturnFailure()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>();
        var query = new GetTenantUserByIdQuery(Guid.NewGuid());

        // Act
        var result = await mediator.GetSingleAsync(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }
}

