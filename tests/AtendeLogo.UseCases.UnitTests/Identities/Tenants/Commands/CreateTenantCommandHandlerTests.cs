using AtendeLogo.UseCases.Identities.Tenants.Commands;

namespace AtendeLogo.UseCases.UnitTests.Identities.Tenants.Commands;

public class CreateTenantCommandHandlerTests : IClassFixture<ServiceProviderMock<AnonymousRole>>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly CreateTenantCommand _validadeCommand;

    public CreateTenantCommandHandlerTests(ServiceProviderMock<AnonymousRole> serviceProviderMock,
        ITestOutputHelper testOutput)
    {
        serviceProviderMock.AddTestOutput(testOutput);

        _serviceProvider = serviceProviderMock;

        var fakePhoneNumber = BrazilianFakeUtils.GenerateFakePhoneNumber();
        var fakeEmail = FakeUtils.GenerateFakeEmail();
        var fakeCpf = BrazilianFakeUtils.GenerateCpf();

        _validadeCommand = new CreateTenantCommand
        {
            Name = "Tenant name",
            FiscalCode = new FiscalCode(fakeCpf),
            TenantName = "Tenant name",
            Email = fakeEmail,
            Password = "Password123!",
            Country = Country.Brazil,
            Language = Language.Portuguese,
            Currency = Currency.BRL,
            BusinessType = BusinessType.CivilRegistryOffice,
            TenantType = TenantType.Company,
            PhoneNumber = new PhoneNumber(fakePhoneNumber)
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
        handlerType.Should().BeOfType<CreateTenantCommandHandler>();
    } 

    [Fact]
    public async Task HandleAsync_ReturnsSuccess()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>();

        var command = _validadeCommand;

        // Act
        var result = await mediator.RunAsync(command, CancellationToken.None);

        // Assert
        result.ShouldBeSuccessful();
    }

    [Fact]
    public async Task HandleAsync_ReturnsFailure_WhenPasswordIsInvalid()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>();
        var command = _validadeCommand with
        {
            Password = "password"
        };

        // Act
        var result = await mediator.TestRunAsync(command );

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public async Task Validate_ShouldHaveError_WhenEmailIsInvalid()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>();
        var command = _validadeCommand with
        {
            Email = "tenant"
        };

        // Act
        var result = await mediator.TestRunAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public async Task Validate_ShouldHaveError_WhenNameIsNotFullName()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>();
        var command = _validadeCommand with
        {
            Name = "First"
        };

        // Act
        var result = await mediator.TestRunAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public async Task Validate_ShouldHaveError_WhenFiscalCodeIsEmpty()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>();
        var command = _validadeCommand with
        {
            FiscalCode = new FiscalCode(string.Empty)
        };

        // Act
        var result = await mediator.TestRunAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FiscalCode.Value);
    }
}

