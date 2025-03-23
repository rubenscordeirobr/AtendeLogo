using AtendeLogo.UseCases.Identities.Users.TenantUsers.Queries;

namespace AtendeLogo.UseCases.UnitTests.Identities.Users.TenantUsers.Queries;

public class GetTenantUserByPhoneNumberQueryHandlerTests : IClassFixture<TenantOwnerUserServiceProviderMock>
{
    private readonly IServiceProvider _serviceProvider;

    public GetTenantUserByPhoneNumberQueryHandlerTests(
        TenantOwnerUserServiceProviderMock serviceProviderMock,
        ITestOutputHelper testOutput)
    {
        serviceProviderMock.AddTestOutput(testOutput);

            _serviceProvider = serviceProviderMock;
    }

    [Fact]
    public void Handle_ShouldBe_GetTenantUserByPhoneNumberQueryHandler()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>() as IRequestMediatorTest;
        var query = new GetTenantUserByPhoneNumberQuery("1234567890");

        // Act
        var handlerType = mediator!.GetRequestHandler(query);

        // Assert
        handlerType.Should().BeOfType<GetTenantUserByPhoneNumberQueryHandler>();
    }

    [Fact]
    public async Task HandleAsync_ReturnSuccess()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>();
        var tenantUserRepository = _serviceProvider.GetRequiredService<ITenantUserRepository>();
        var tenantUser = await tenantUserRepository.GetByPhoneNumberAsync(SystemTenantConstants.PhoneNumber);


        Guard.NotNull(tenantUser);

        var query = new GetTenantUserByPhoneNumberQuery(tenantUser.PhoneNumber.Number);

        // Act
        var result = await mediator.GetAsync(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        var tenantUserResponse = result.Value
            .Should()
            .BeOfType<TenantUserResponse>()
            .Subject;

        tenantUserResponse.PhoneNumber.Should().Be(tenantUser.PhoneNumber);
    }

    [Fact]
    public async Task HandleAsync_ReturnFailure()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>();
        var query = new GetTenantUserByPhoneNumberQuery("0987654321");

        // Act
        var result = await mediator.GetAsync(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }
}

