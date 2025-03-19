using AtendeLogo.UseCases.Identities.Users.TenantUsers.Commands;

namespace AtendeLogo.UseCases.UnitTests.Identities.Users.TenantUsers.Commands;
 
public class DeleteTenantUserCommandHandlerTests : IClassFixture<TenantOwnerUserServiceProviderMock>
{
    private readonly IRequestMediatorTest _mediator;
    private readonly DeleteTenantUserCommand _validCommand;

    public DeleteTenantUserCommandHandlerTests(TenantOwnerUserServiceProviderMock serviceProvider)
    {
        _mediator = (IRequestMediatorTest)serviceProvider.GetRequiredService<IRequestMediator>();
        _validCommand = new DeleteTenantUserCommand(SystemTenantConstants.OwnerUser_Id);
    }

    [Fact]
    public void Handler_ShouldBe_DeleteTenantUserCommandHandler()
    {
        // Act
        var handlerType = _mediator.GetRequestHandler(_validCommand);

        // Assert
        handlerType.Should().BeOfType<DeleteTenantUserCommandHandler>();
    }

    [Fact]
    public async Task HandleAsync_ReturnNotFoundFailure()
    {
        // Arrange
        var tenantUserId = Guid.NewGuid();
        var command = new DeleteTenantUserCommand(tenantUserId);

        // Act
        var result = await _mediator.RunAsync(command, CancellationToken.None);

        // Assert
        result.ShouldBeFailure<NotFoundError>();
    }

    [Fact]
    public async Task HandleAsync_ReturnNotUnauthorizedFailure()
    {
        // Arrange
        var anonymousServiceProvider = new AnonymousServiceProviderMock();
        var mediator = (IRequestMediatorTest)anonymousServiceProvider.GetRequiredService<IRequestMediator>();

        // Act
        var result = await mediator.RunAsync(_validCommand, CancellationToken.None);

        // Assert
        result.ShouldBeFailure<UnauthorizedError>();
    }

    [Fact]
    public async Task HandleAsync_ReturnSuccess()
    {
        // Act
        var result = await _mediator.RunAsync(_validCommand, CancellationToken.None);

        // Assert
        result.ShouldBeSuccessful();
    }
}
