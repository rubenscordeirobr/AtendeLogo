using AtendeLogo.Application.Contracts.Persistence.Identity;
using AtendeLogo.UseCases.Identities.Users.TenantUsers.Queries;

namespace AtendeLogo.UseCases.UnitTests.Identities.Users.TenantUsers;

public class GetTenantUserByEmailOrPhoneNumberQueryHandlerTests : IClassFixture<TenantUserServiceProviderMock>
{
    private readonly IServiceProvider _serviceProvider;

    public GetTenantUserByEmailOrPhoneNumberQueryHandlerTests(TenantUserServiceProviderMock serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    [Fact]
    public void Handle_ShouldBe_GetTenantUserByEmailOrPhoneNumberQueryHandler()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>() as IRequestMediatorTest;
        var query = new GetTenantUserByEmailOrPhoneNumberQuery("test@example.com", "1234567890");

        // Act
        var handlerType = mediator!.GetRequestHandler(query);

        // Assert
        handlerType.Should().BeOfType<GetTenantUserByEmailOrPhoneNumberQueryHandler>();
    }

    [Fact]
    public async Task HandleAsync_ReturnSuccess_ByEmail()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>();
        var tenantUserRepository = _serviceProvider.GetRequiredService<ITenantUserRepository>();
        var tenantUser = await tenantUserRepository.GetByEmailAsync(SystemTenantConstants.TenantSystemEmail);

        Guard.NotNull(tenantUser);

        var query = new GetTenantUserByEmailOrPhoneNumberQuery(tenantUser.Email, null!);

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
    public async Task HandleAsync_ReturnSuccess_ByPhoneNumber()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>();
        var tenantUserRepository = _serviceProvider.GetRequiredService<ITenantUserRepository>();
        var tenantUser = await tenantUserRepository.GetByPhoneNumberAsync(SystemTenantConstants.PhoneNumber);

        Guard.NotNull(tenantUser);

        var query = new GetTenantUserByEmailOrPhoneNumberQuery(null!, tenantUser.PhoneNumber.Number);

        // Act
        var result = await mediator.GetSingleAsync(query, CancellationToken.None);

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
        var query = new GetTenantUserByEmailOrPhoneNumberQuery("nonexistent@example.com", "0987654321");

        // Act
        var result = await mediator.GetSingleAsync(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }
}

