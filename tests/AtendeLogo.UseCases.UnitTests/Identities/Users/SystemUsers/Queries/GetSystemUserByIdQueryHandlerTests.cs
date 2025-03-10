﻿using AtendeLogo.UseCases.Identities.Users.SystemUsers.Queries;

namespace AtendeLogo.UseCases.UnitTests.Identities.Users.SystemUsers.Queries;

public class GetSystemUserByIdQueryHandlerTests : IClassFixture<AnonymousServiceProviderMock>
{
    private readonly IServiceProvider _serviceProvider;

    public GetSystemUserByIdQueryHandlerTests(AnonymousServiceProviderMock serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    [Fact]
    public void HandleType_ShouldBe_GetSystemUserByIdQueryHandler()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>() as IRequestMediatorTest;
        var query = new GetSystemUserByIdQuery(Guid.NewGuid());

        // Act
        var handlerType = mediator!.GetRequestHandler(query);

        // Assert
        handlerType.Should().BeOfType<GetSystemUserByIdQueryHandler>();
    }

    [Fact]
    public async Task HandleAsync_ReturnSuccess()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>();
        var query = new GetSystemUserByIdQuery(AnonymousIdentityConstants.AnonymousUser_Id);

        // Act
        var result = await mediator.GetSingleAsync(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        var systemUser = result.Value
            .Should()
            .BeOfType<SystemUserResponse>()
            .Subject;

        systemUser.Id.Should().Be(AnonymousIdentityConstants.AnonymousUser_Id);
    }

    [Fact]
    public async Task HandleAsync_ReturnFailure()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>();
        var query = new GetSystemUserByIdQuery(Guid.NewGuid());

        // Act
        var result = await mediator.GetSingleAsync(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }
}
