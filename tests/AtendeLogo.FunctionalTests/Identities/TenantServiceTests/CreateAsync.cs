using AtendeLogo.Shared.Enums;
using AtendeLogo.Shared.ValueObjects;
using AtendeLogo.UseCases.Identities.Tenants.Commands;

namespace AtendeLogo.FunctionalTests.Identities;

public partial class TenantServiceTests
{
    [Fact]
    public async Task CreateTenant_ShouldBeSuccessful()
    {
        // Arrange
        var fakePhoneNumber = BrazilianFakeUtils.GenerateFakePhoneNumber();
        var fakeEmail = FakeUtils.GenerateFakeEmail();
        var fakeCpf = BrazilianFakeUtils.GenerateCpf();

        var createCommand = new CreateTenantCommand
        {
            Name = "Tenant name",
            FiscalCode = new FiscalCode(fakeCpf),
            TenantName = "Tenant name",
            Email = fakeEmail,
            Password = "Password123!",
            Country = Country.Brazil,
            Language = Language.PortugueseBrazil,
            Currency = Currency.BRL,
            BusinessType = BusinessType.CivilRegistryOffice,
            TenantType = TenantType.Company,
            PhoneNumber = new PhoneNumber(fakePhoneNumber)
        };

        // Act
        var result = await _clientService.CreateAsync(
            createCommand,
            CancellationToken.None);

        // Assert
        result.ShouldBeSuccessful();
        result.Value!.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateTenant_ShouldBeFailure()
    {
        // Arrange
        var fakePhoneNumber = BrazilianFakeUtils.GenerateFakePhoneNumber();
        var fakeCpf = BrazilianFakeUtils.GenerateCpf();

        var createCommand = new CreateTenantCommand
        {
            Name = "Tenant name",
            FiscalCode = new FiscalCode(fakeCpf),
            TenantName = "Tenant name",
            Email = string.Empty,
            Password = "Password123!",
            Country = Country.Brazil,
            Language = Language.PortugueseBrazil,
            Currency = Currency.BRL,
            BusinessType = BusinessType.CivilRegistryOffice,
            TenantType = TenantType.Company,
            PhoneNumber = new PhoneNumber(fakePhoneNumber)
        };

        // Act
        var result = await _clientService.CreateAsync(createCommand, CancellationToken.None);

        // Assert
        result.ShouldBeFailure<ValidationError>();
    }
}

