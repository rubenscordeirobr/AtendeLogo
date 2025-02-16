using AtendeLogo.Shared.Contantes;
using AtendeLogo.UseCases.Identities.Users.SystemUsers.Queries;

namespace AtendeLogo.Application.UnitTests.UseCases.Identities.Users.SystemUsers.Queries;

public class GetSystemUserByIdQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnSuccess()
    {
        // Arrange
        var serviceProvider = new AnonymousServiceProviderMock();
        var mediator = serviceProvider.GetRequiredService<IRequestMediator>();
        var query = new GetSystemUserByIdQuery { Id = AnonymousConstants.AnonymousUser_Id };

        // Act
        var result = await mediator.GetSingleAsync(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        var systemUser = result.Value
            .Should()
            .BeOfType<SystemUserResponse>()
            .Subject;

        systemUser.Id.Should().Be(AnonymousConstants.AnonymousUser_Id);
    }

    [Fact]
    public async Task HandleAsync_ReturnFailure()
    {
        // Arrange
        var serviceProvider = new AnonymousServiceProviderMock();
        var mediator = serviceProvider.GetRequiredService<IRequestMediator>();
        var query = new GetSystemUserByIdQuery { Id = Guid.NewGuid() };
        
        // Act
        var result = await mediator.GetSingleAsync(query, CancellationToken.None);
      
        // Assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }
}
