using AtendeLogo.UseCases.Identities.Tenants.Commands;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AtendeLogo.Application.UnitTests.UseCases.Identities.Tenants.Commands;

public class CreateTenantCommandHandlerTests : IClassFixture<AnonymousServiceProviderMock>
{
    private IServiceProvider _serviceProvider;
    private readonly CreateTenantCommand _validadeCommand;

    public CreateTenantCommandHandlerTests(AnonymousServiceProviderMock serviceProvide)
    {
        _serviceProvider = serviceProvide;
        _validadeCommand = new CreateTenantCommand
        {
            ClientRequestId = Guid.NewGuid(),
            Name = "Tenant name",
            FiscalCode = "04866748940",
            TenantName = "Tenant name",
            Email = "tenant1@atendelogo.com",
            Password = "Password123!",
            Country = Country.Brazil,
            Language = Language.Portuguese,
            Currency = Currency.BRL,
            BusinessType = BusinessType.CivilRegistryOffice,
            TenantType = TenantType.Company,
            PhoneNumber = new PhoneNumber("+55 42 99977 1020")
        };
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
        var result = await mediator.RunAsync(command);

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
        var result = await mediator.RunAsync(command);

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
        var result = await mediator.RunAsync(command);

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
            FiscalCode = string.Empty
        };

        // Act
        var result = await mediator.RunAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FiscalCode);
    }
}

