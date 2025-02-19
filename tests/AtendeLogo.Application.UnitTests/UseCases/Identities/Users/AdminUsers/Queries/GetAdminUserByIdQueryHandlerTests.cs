using AtendeLogo.Application.Contracts.Persistence.Identity;
using AtendeLogo.Common;
using AtendeLogo.UseCases.Identities.Users.AdminUsers.Queries;

namespace AtendeLogo.Application.UnitTests.UseCases.Identities.Users.AdminUsers.Queries;

public class GetAdminUserByIdQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnSuccess()
    {
        // Arrange
        var serviceProvider = new AnonymousServiceProviderMock();
        var mediator = serviceProvider.GetRequiredService<IRequestMediator>();
        var guidAdminUserId = Guid.NewGuid();

        var adminUserRepository = serviceProvider.GetRequiredService<IAdminUserRepository>();
        var superAdminUser = await adminUserRepository.GetByEmailAsync("superadmin@atendelogo.com.br");

        Guard.NotNull(superAdminUser);

        var query = new GetAdminUserByIdQuery { Id = superAdminUser.Id };

        // Act
        var result = await mediator.GetSingleAsync(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        var adminUser = result.Value
            .Should()
            .BeOfType<AdminUserResponse>()
            .Subject;

        adminUser.Id.Should().Be(superAdminUser.Id);
    }

    [Fact]
    public async Task HandleAsync_ReturnFailure()
    {
        // Arrange
        var serviceProvider = new AnonymousServiceProviderMock();
        var mediator = serviceProvider.GetRequiredService<IRequestMediator>();
        var query = new GetAdminUserByIdQuery { Id = Guid.NewGuid() };

        // Act
        var result = await mediator.GetSingleAsync(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }
}

