using AtendeLogo.UseCases.Identities.Tenants.Commands;
using AtendeLogo.UseCases.UnitTests.TestSupport;

namespace AtendeLogo.UseCases.UnitTests.Identities.Tenants.Commands;

public class UpdateDefaultTenantAddressCommandHandlerTests : IClassFixture<AnonymousServiceProviderMock>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly UpdateDefaultTenantAddressCommand _validadeCommand;

    public UpdateDefaultTenantAddressCommandHandlerTests(AnonymousServiceProviderMock serviceProvide)
    {
        _serviceProvider = serviceProvide;
        _validadeCommand = new UpdateDefaultTenantAddressCommand
        {
            ClientRequestId = Guid.NewGuid(),
            Tenant_Id = Guid.NewGuid(),
            AddressName = "Address name",
            Address = new AddressDto
            {
                Street = "Street",
                Number = "123",
                Complement = "Complement",
                Neighborhood = "Neighborhood",
                City = "City",
                State = "PR",
                ZipCode = "8570555",
                Country = Country.Brazil
            }
        };
    }

    [Fact]
    public void Handler_ShouldBe_CreateTenantCommandHandler()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>() as IRequestMediatorTest;
         
        // Act
        var handlerType = mediator!.GetRequestHandler(_validadeCommand);

        // Assert
        handlerType.Should().BeOfType<UpdateDefaultTenantAddressCommandHandler>();
    }

    [Fact]
    public async Task HandleAsync_ReturnsSuccess()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>();
        var command = _validadeCommand with
        {
            Tenant_Id = SystemTenantConstants.TenantSystem_Id
        };

        // Act
        var result = await mediator.RunAsync(command, CancellationToken.None);

        // Assert
        result.ShouldBeSuccessful();
    }

    [Fact]
    public async Task HandleAsync_ReturnsFailure_WhenAddressNameIsInvalid()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>();
        var command = _validadeCommand with
        {
            AddressName = String.Empty
        };

        // Act
        var result = await mediator.TestRunAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.AddressName);
    }
}

