namespace AtendeLogo.FunctionalTests.Identities;

public partial class TenantValidationServiceTests
{

    [Fact]
    public async Task IsFiscalCodeUnique_ShouldReturnTrue()
    {
        var fiscalCode = BrazilianFakeUtils.GenerateCpf();

        var result = await _clientService.IsFiscalCodeUniqueAsync(
            fiscalCode);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsFiscalCodeUnique_ShouldReturnFalse()
    {
        var fiscalCode = SystemTenantConstants.FiscalCode;

        var result = await _clientService.IsFiscalCodeUniqueAsync(
            fiscalCode);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task IsFiscalCodeUnique_WithFormatNumber_ShouldReturnFalse()
    {
        var fiscalCode = SystemTenantConstants.FiscalCode;
        var fiscalCodeFormatted = BrazilianFormattingUtils.FormatCnpj(fiscalCode);

        var result = await _clientService.IsFiscalCodeUniqueAsync(
            fiscalCodeFormatted);

        result.Should().BeFalse();
    }


    [Fact]
    public async Task IsFiscalCodeUnique_WithTenant_ShouldReturnTrue()
    {
        var fiscalCode = BrazilianFakeUtils.GenerateCpf();

        var result = await _clientService.IsFiscalCodeUniqueAsync(
            SystemTenantConstants.Tenant_Id,
            fiscalCode);

        result.Should().BeTrue();
    }
}
