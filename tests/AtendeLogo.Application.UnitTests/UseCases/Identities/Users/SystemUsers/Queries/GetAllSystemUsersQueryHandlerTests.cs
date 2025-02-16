using AtendeLogo.UseCases.Identities.Users.SystemUsers.Queries;

namespace AtendeLogo.Application.UnitTests.UseCases.Identities.Users.SystemUsers.Queries;

public class GetAllSystemUsersQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnSuccess()
    {
        // Arrange
        var serviceProvider = new AnonymousServiceProviderMock();
        var mediator = serviceProvider.GetRequiredService<IRequestMediator>();
        var query = new GetAllSystemUsersQuery();

        // Act
        var result = await mediator.GetManyAsync(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();

        var systemUser = result.First()
            .Should()
            .BeOfType<SystemUserResponse>()
            .Subject;

        systemUser.Id.Should().NotBeEmpty();
    }
 
}

