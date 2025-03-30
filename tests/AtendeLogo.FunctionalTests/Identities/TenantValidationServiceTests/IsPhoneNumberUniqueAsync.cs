﻿namespace AtendeLogo.FunctionalTests.Identities;

public partial class TenantValidationServiceTests
{
    [Fact]
    public async Task IsPhoneNumberUnique_ShouldReturnTrue()
    {
        var phoneNumber = BrazilianFakeUtils.GenerateFakePhoneNumber();

        var result = await _clientService.IsPhoneNumberUniqueAsync(
            phoneNumber);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsPhoneNumberUnique_ShouldReturnFalse()
    {
        var phoneNumber = SystemTenantConstants.PhoneNumber;

        var result = await _clientService.IsPhoneNumberUniqueAsync(
            phoneNumber);

        result.Should()
            .BeFalse();
    }

    [Fact]
    public async Task IsPhoneNumberUnique_WithFormattedNumber_ShouldReturnFalse()
    {
        var phoneNumber = SystemTenantConstants.PhoneNumber;
        var phoneNumberFormatted = BrazilianFormattingUtils.FormatPhone(
            phoneNumber, 
            internationalFormat: true);

        var result = await _clientService.IsPhoneNumberUniqueAsync(
            phoneNumberFormatted);

        result.Should()
            .BeFalse();
    }

    [Fact]
    public async Task IsPhoneNumberUnique_WithTenantAndOwner_ShouldReturnTrue()
    {
        var phoneNumber = BrazilianFakeUtils.GenerateFakePhoneNumber();

        var result = await _clientService.IsPhoneNumberUniqueAsync(
            SystemTenantConstants.Tenant_Id,
            SystemTenantConstants.User_Id,
            phoneNumber);

        result.Should().BeTrue();
    }
}
