using AtendeLogo.Application.Contracts.Persistence.Identity;
using AtendeLogo.UseCases.Identities.Users.AdminUsers.Queries;

namespace AtendeLogo.UseCases.UnitTests.Identities.Users.AdminUsers.Queries;

public class GetAdminUserByEmailOrPhoneNumberQueryHandlerTests : IClassFixture<AnonymousServiceProviderMock>
{
    private readonly IServiceProvider _serviceProvider;

    public GetAdminUserByEmailOrPhoneNumberQueryHandlerTests(AnonymousServiceProviderMock serviceProvider)
    {
        this._serviceProvider = serviceProvider;
    }

    [Fact]
    public void Handle_ShouldBe_GetAdminUserByEmailOrPhoneNumberQueryHandler()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>() as IRequestMediatorTest;
        var query = new GetAdminUserByEmailOrPhoneNumberQuery("test@example.com", "1234567890");

        // Act
        var handlerType = mediator!.GetRequestHandler(query);

        // Assert
        handlerType.Should().BeOfType<GetAdminUserByEmailOrPhoneNumberQueryHandler>();
    }

    [Fact]
    public async Task HandleAsync_ReturnSuccess()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>();
        var adminUserRepository = _serviceProvider.GetRequiredService<IAdminUserRepository>();
        var superAdminUser = await adminUserRepository.GetByEmailAsync("superadmin@atendelogo.com.br");

        Guard.NotNull(superAdminUser);

        var query = new GetAdminUserByEmailOrPhoneNumberQuery(
            superAdminUser.Email, superAdminUser.PhoneNumber.Number);

        // Act
        var result = await mediator.GetSingleAsync(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        var adminUser = result.Value
            .Should()
            .BeOfType<AdminUserResponse>()
            .Subject;

        adminUser.Email.Should().Be(superAdminUser.Email);
        adminUser.PhoneNumber.Should().Be(superAdminUser.PhoneNumber);
    }

    [Fact]
    public async Task HandleAsync_ReturnFailure()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>();
        var query = new GetAdminUserByEmailOrPhoneNumberQuery("nonexistent@example.com", "0000000000");

        // Act
        var result = await mediator.GetSingleAsync(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }
}

