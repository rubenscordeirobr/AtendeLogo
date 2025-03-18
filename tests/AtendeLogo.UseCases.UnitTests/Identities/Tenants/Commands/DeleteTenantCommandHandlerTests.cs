using AtendeLogo.UseCases.Identities.Tenants.Commands;


namespace AtendeLogo.UseCases.UnitTests.Identities.Tenants.Commands;

public class DeleteTenantCommandHandlerTests : IClassFixture<SystemAdminUserServiceProviderMock>
{
    private readonly IRequestMediatorTest _mediator;
    private readonly DeleteTenantCommand _validadeCommand;

    public DeleteTenantCommandHandlerTests(SystemAdminUserServiceProviderMock serviceProvider)
    {
        _mediator = (IRequestMediatorTest)serviceProvider.GetRequiredService<IRequestMediator>();
        _validadeCommand = new DeleteTenantCommand(SystemTenantConstants.Tenant_Id);
    }

    [Fact]
    public void Handler_ShouldBe_DeleteTenantCommandHandler()
    {
        // Act
        var handlerType = _mediator!.GetRequestHandler(_validadeCommand);

        // Assert
        handlerType.Should().BeOfType<DeleteTenantCommandHandler>();
    }

    [Fact]
    public async Task HandleAsync_ReturnNotFoundFailure()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var command = new DeleteTenantCommand(tenantId);

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
        var result = await mediator.RunAsync(_validadeCommand, CancellationToken.None);

        // Assert
        result.ShouldBeFailure<UnauthorizedError>();
    }

    [Fact]
    public async Task HandleAsync_ReturnSuccess()
    {
        // Arrange
        var result = await _mediator.RunAsync(_validadeCommand, CancellationToken.None);

        // Assert
        result.ShouldBeSuccessful();
    }
}
 
