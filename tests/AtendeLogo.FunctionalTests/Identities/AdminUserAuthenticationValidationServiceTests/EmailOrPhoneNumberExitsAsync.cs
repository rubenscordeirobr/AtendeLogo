﻿namespace AtendeLogo.FunctionalTests.Identities;

public partial class AdminUserAuthenticationValidationServiceTests
{

    [Fact]
    public async Task EmailOrPhoneNumberExists_WithExistingEmail_ShouldReturnTrue()
    {
        //Arrange
        var email = DefaultAdminUserConstants.Email;

        //Act
        var result = await _clientService.EmailOrPhoneNumberExitsAsync(
            email);

        //Assert
        result.Should()
            .BeTrue();
    }

    [Fact]
    public async Task EmailOrPhoneNumberExists_WithExistingPhoneNumber_ShouldReturnTrue()
    {
        //Arrange
        var phoneNumber = DefaultAdminUserConstants.PhoneNumber;

        //Act
        var result = await _clientService.EmailOrPhoneNumberExitsAsync(
            phoneNumber);

        //Assert
        result.Should()
            .BeTrue();
    }

    [Fact]
    public async Task EmailOrPhoneNumberExists_WithExistingFormattedPhoneNumber_ShouldReturnTrue()
    {
        //Arrange
        var formattedPhoneNumber = BrazilianFormattingUtils.FormatPhone(
            DefaultAdminUserConstants.PhoneNumber,
            internationalFormat: true);

        //Act
        var result = await _clientService.EmailOrPhoneNumberExitsAsync(
            formattedPhoneNumber);

        //Assert
        result.Should()
            .BeTrue();
    }

    [Fact]
    public async Task EmailOrPhoneNumberExists_WithNonExistingEmail_ShouldReturnFalse()
    {
        //Arrange
        var email = FakeUtils.GenerateFakeEmail();

        //Act
        var result = await _clientService.EmailOrPhoneNumberExitsAsync(
            email);

        //Assert
        result.Should()
            .BeFalse();
    }

    [Fact]
    public async Task EmailOrPhoneNumberExists_WithNonExistingPhoneNumber_ShouldReturnFalse()
    {
        //Arrange
        var phoneNumber = BrazilianFakeUtils.GenerateFakePhoneNumber();

        //Act
        var result = await _clientService.EmailOrPhoneNumberExitsAsync(
            phoneNumber);

        //Assert
        result.Should()
            .BeFalse();
    }
}
