using AtendeLogo.Shared.Contracts;
using AtendeLogo.UseCases.Common.Validations;
using AtendeLogo.UseCases.Identities.Tenants.Commands;
using AtendeLogo.UseCases.Identities.Tenants.Services;
using FluentValidation;
using Moq;

namespace AtendeLogo.Application.UnitTests.UseCases.Identities.Tenants.Commands;

public class CreateTenantCommandValidatorTests : IClassFixture<AnonymousServiceProviderMock>
{
    private IServiceProvider _serviceProvider;
    private readonly IValidator<CreateTenantCommand> _validator;
    private readonly CreateTenantCommand _validadeCommand;

    public CreateTenantCommandValidatorTests(AnonymousServiceProviderMock serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _validator = serviceProvider.GetRequiredService<IValidator<CreateTenantCommand>>();

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
    public async Task ValidationResult_ShouldBeValid()
    {
        // Arrange
        var command = _validadeCommand;

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should()
            .BeTrue();

        result.Errors.Should()
            .BeEmpty();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("A")]
    [InlineData("AB")]
    [InlineData("first")]
    [InlineData("ThisNameIsWayTooLongForTheValidationToPass")]
    public async Task ValidationResult_ShouldHaveError_When_Name_IsInvalid(string? name)
    {
        // Arrange
        var command = _validadeCommand with
        {
            Name = name!
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("abc")]
    [InlineData(ContantesTest.StringToLongMoreThan100)]
    public async Task ValidationResult_ShouldHaveError_When_TenantName_IsInvalid(
        string? tenantName)
    {
        // Arrange
        var command = _validadeCommand with
        {
            TenantName = tenantName!
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TenantName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("invalidemail")]
    [InlineData("invalid@")]
    [InlineData("invalid@com .10")]
    [InlineData("1 invalid@com .10")]
    public async Task ValidationResult_ShouldHaveError_When_Email_IsInvalid(string? email)
    {
        // Arrange
        var command = _validadeCommand with
        {
            Email = email!
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("132")]
    [InlineData("13245674")]
    [InlineData("15616159616")]
    public async Task ValidationResult_ShouldHaveError_When_FiscalCode_IsInvalid(string? fiscalCode)
    {
        // Arrange
        var command = _validadeCommand with
        {
            FiscalCode = fiscalCode!
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FiscalCode);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("132")]
    [InlineData("13245674")]
    public async Task ValidationResult_ShouldHaveError_When_PhoneNumber_IsInvalid(string? phoneNumber)
    {
        // Arrange
        var command = _validadeCommand with
        {
            PhoneNumber = new PhoneNumber(phoneNumber!)
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber.Number);
    }

    [Theory]
    [InlineData(null)]
    [InlineData((Country)(-1))]
    [InlineData(Country.Unknown)]
    public async Task ValidationResult_ShouldHaveError_When_Country_IsInvalid(Country? country)
    {
        // Arrange
        var command = _validadeCommand with
        {
            Country = country.GetValueOrDefault()
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Country);
    }

    [Theory]
    [InlineData(null)]
    [InlineData((Language)(-1))]
    public async Task ValidationResult_ShouldHaveError_When_Language_IsInvalid(Language? language)
    {
        // Arrange
        var command = _validadeCommand with
        {
            Language = language.GetValueOrDefault()
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Language);

    }

    [Theory]
    [InlineData(null)]
    [InlineData((Currency)(-1))]
    public async Task ValidationResult_ShouldHaveError_When_Currency_IsInvalid(Currency? currency)
    {
        // Arrange
        var command = _validadeCommand with
        {
            Currency = currency.GetValueOrDefault()
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Currency);
    }

    [Theory]
    [InlineData(null)]
    [InlineData((BusinessType)(-1))]

    public async Task ValidationResult_ShouldHaveError_When_BusinessType_IsInvalid(BusinessType? businessType)
    {
        // Arrange
        var command = _validadeCommand with
        {
            BusinessType = businessType.GetValueOrDefault()
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.BusinessType);
    }

    [Theory]
    [InlineData(null)]
    [InlineData((TenantType)(-1))]
    public async Task ValidationResult_ShouldHaveError_When_TenantType_IsInvalid(TenantType? tenantType)
    {
        // Arrange
        var command = _validadeCommand with
        {
            TenantType = tenantType.GetValueOrDefault()
        };
        // Act
        var result = await _validator.ValidateAsync(command);
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TenantType);
    }
     
    [Fact]
    public async Task ValidationResult_ShouldEmailBeUnique()
    {
        // Arrange
        var localizer = _serviceProvider.GetRequiredService<IJsonStringLocalizer<ValidationMessages>>();
        
        var tenantValidationSetup = new Mock<ITenantValidationService>();
        tenantValidationSetup
            .Setup(x => x.IsEmailUniqueAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        tenantValidationSetup.Setup(x => x.IsFiscalCodeUniqueAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validator = new CreateTenantCommandValidator(
            localizer,
            tenantValidationSetup.Object);

        var command = _validadeCommand with
        {
            Email = "alreadyexists@atendelogo.com"
        };

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public async Task ValidationResult_ShouldFiscalCodeBeUnique()
    {
        // Arrange
        var localizer = _serviceProvider.GetRequiredService<IJsonStringLocalizer<ValidationMessages>>();
        
        var tenantValidationSetup = new Mock<ITenantValidationService>();
        tenantValidationSetup
            .Setup(x => x.IsEmailUniqueAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        tenantValidationSetup.Setup(x => x.IsFiscalCodeUniqueAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new CreateTenantCommandValidator(
            localizer,
            tenantValidationSetup.Object);

        var command = _validadeCommand with
        {
            FiscalCode = "04866748940"
        };

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FiscalCode);
    }
}
