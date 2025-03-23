﻿using AtendeLogo.UseCases.Identities.Users.SystemUsers.Queries;

namespace AtendeLogo.UseCases.UnitTests.Identities.Users.SystemUsers.Queries;

public class GetAllSystemUsersQueryHandlerTests : IClassFixture<AnonymousServiceProviderMock>
{
    private readonly IServiceProvider _serviceProvider;

    public GetAllSystemUsersQueryHandlerTests(AnonymousServiceProviderMock serviceProviderMock,
        ITestOutputHelper testOutput)
    {
        serviceProviderMock.AddTestOutput(testOutput);

        _serviceProvider = serviceProviderMock;
    }

    [Fact]
    public void HandleType_ShouldBe_GetAllSystemUsersQueryHandler()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>() as IRequestMediatorTest;
        var query = new GetAllSystemUsersQuery();

        // Act
        var handlerType = mediator!.GetRequestHandler(query);

        // Assert
        handlerType.Should().BeOfType<GetAllSystemUsersQueryHandler>();
    }

    [Fact]
    public async Task HandleAsync_ReturnSuccess()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>();
        var query = new GetAllSystemUsersQuery();

        // Act
        var result = await mediator.GetManyAsync(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().NotBeEmpty();

        result.Value.Should()
            .AllBeOfType<SystemUserResponse>();

    }
}

