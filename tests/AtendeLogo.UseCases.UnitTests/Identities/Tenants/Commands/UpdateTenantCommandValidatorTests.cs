using AtendeLogo.UseCases.Identities.Tenants.Commands;

namespace AtendeLogo.UseCases.UnitTests.Identities.Tenants.Commands;

public class UpdateTenantCommandValidatorTests : IClassFixture<ServiceProviderMock<AnonymousRole>>
{
    private readonly IValidator<UpdateTenantCommand> _validator;
    private readonly UpdateTenantCommand _validCommand;

    public UpdateTenantCommandValidatorTests(ServiceProviderMock<AnonymousRole> serviceProviderMock,
        ITestOutputHelper testOutput)
    {
        serviceProviderMock.AddTestOutput(testOutput);

        _validator = serviceProviderMock.GetRequiredService<IValidator<UpdateTenantCommand>>();
        _validCommand = new UpdateTenantCommand
        {
            Tenant_Id = SystemTenantConstants.Tenant_Id,
            Name = "John Doe",
            FiscalCode = new FiscalCode("76776292027"),
            Country = Country.Brazil,
            Culture = Culture.PtBr,
            Currency = Currency.BRL,
            BusinessType = BusinessType.CivilRegistryOffice,
            TenantType = TenantType.Company,
        };
    }

    [Fact]
    public void Validator_ShouldBe_UpdateTenantCommandValidator()
    {
        _validator.Should().BeOfType<UpdateTenantCommandValidator>();
    }

    [Fact]
    public async Task ValidationResult_ShouldBeValid()
    {
        var result = await _validator.TestValidateAsync(_validCommand);
        result.Errors.Should().BeEmpty("Command should be valid.");
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("A")]
    [InlineData("John")]
    [InlineData("ThisNameIsWayTooLongForTheValidationToPassBecauseItExceedsTheMaximumAllowedLengthForNameField")]
    public async Task ValidationResult_ShouldHaveError_When_Name_IsInvalid(string? name)
    {
        var command = _validCommand with { Name = name! };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("123")]
    [InlineData("12345678901234567890")]
    public async Task ValidationResult_ShouldHaveError_When_FiscalCode_IsInvalid(string? fiscalCode)
    {
        var command = _validCommand with { FiscalCode = new FiscalCode(fiscalCode!)! };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.FiscalCode.Value);
    }

    [Theory]
    [InlineData((Country)(-1))]
    [InlineData(Country.Unknown)]
    public async Task ValidationResult_ShouldHaveError_When_Country_IsInvalid(Country country)
    {
        var command = _validCommand with { Country = country };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.Country);
    }

    [Theory]
    [InlineData((Culture)(-1))]
    [InlineData(Culture.Default)]
    public async Task ValidationResult_ShouldHaveError_When_Culture_IsInvalid(Culture culture)
    {
        var command = _validCommand with { Culture = culture };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.Culture);
    }

    [Theory]
    [InlineData((Currency)(-1))]
    public async Task ValidationResult_ShouldHaveError_When_Currency_IsInvalid(Currency currency)
    {
        var command = _validCommand with { Currency = currency };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.Currency);
    }

    [Theory]
    [InlineData((BusinessType)(-1))]
    public async Task ValidationResult_ShouldHaveError_When_BusinessType_IsInvalid(BusinessType businessType)
    {
        var command = _validCommand with { BusinessType = businessType };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.BusinessType);
    }

    [Theory]
    [InlineData((TenantType)(-1))]
    public async Task ValidationResult_ShouldHaveError_When_TenantType_IsInvalid(TenantType tenantType)
    {
        var command = _validCommand with { TenantType = tenantType };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.TenantType);
    }
}
