﻿using AtendeLogo.UseCases.Identities.Authentications.Commands;

namespace AtendeLogo.FunctionalTests.Identities;

public partial class AdminUserAuthenticationServiceTests
{
    [Fact]
    public async Task LoginAsync_WhenValidCredentials_ShouldBeSuccessful()
    {
        // Arrange
         
        ClearCache(); // To avoid the TooManyRequestsError

        var email = DefaultAdminUserConstants.Email;
        var testPassword = DefaultAdminUserConstants.TestPassword;

        var command = new AdminUserLoginCommand
        {
            EmailOrPhoneNumber = email,
            Password = testPassword,
            KeepSession = true
        };

        // Act
        var result = await _clientService.LoginAsync(command);

        // Assert
        result.ShouldBeSuccessful();
    }

    [Fact]
    public async Task LoginAsync_WhenInvalidEmailCredentials_ShouldBeFailure()
    {
        // Arrange
        var invalidEmail = FakeUtils.GenerateFakeEmail();
        var testPassword = DefaultAdminUserConstants.TestPassword;

        var command = new AdminUserLoginCommand
        {
            EmailOrPhoneNumber = invalidEmail,
            Password = testPassword,
            KeepSession = true
        };

        // Act
        var result = await _clientService.LoginAsync(command);

        // Assert
        result.ShouldBeFailure<ValidationError>();
    }

    [Fact]
    public async Task LoginAsync_WhenInvalidPasswordCredentials_ShouldBeFailure()
    {
        ClearCache(); // To avoid the TooManyRequestsError

        // Arrange
        var email = DefaultAdminUserConstants.Email;
        var invalidPassword = "invalidPassword";

        var command = new AdminUserLoginCommand
        {
            EmailOrPhoneNumber = email,
            Password = invalidPassword,
            KeepSession = true
        };

        // Act
        var result = await _clientService.LoginAsync(command);

        // Assert
        result.ShouldBeFailure<AuthenticationError>();
    }

    [Fact]
    public async Task LoginAsync_WhenManyRequests_ShouldBeFailure()
    {
        // Arrange
        var email = DefaultAdminUserConstants.Email;
        var invalidPassword = "invalidPassword";

        var command = new AdminUserLoginCommand
        {
            EmailOrPhoneNumber = email,
            Password = invalidPassword,
            KeepSession = true
        };

        // Act
        var attempt1 = await _clientService.LoginAsync(command);
        var attempt2 = await _clientService.LoginAsync(command);
        var attempt3 = await _clientService.LoginAsync(command);
        var attempt4 = await _clientService.LoginAsync(command);
        var attempt5 = await _clientService.LoginAsync(command);

        var result = await _clientService.LoginAsync(command);

        // Assert
        attempt1.ShouldBeFailureForErrors<AuthenticationError, TooManyRequestsError>(isAssignableTo: true);
        attempt2.ShouldBeFailureForErrors<AuthenticationError, TooManyRequestsError>(isAssignableTo: true);
        attempt3.ShouldBeFailureForErrors<AuthenticationError, TooManyRequestsError>(isAssignableTo: true);
        attempt4.ShouldBeFailureForErrors<AuthenticationError, TooManyRequestsError>(isAssignableTo: true);
        attempt5.ShouldBeFailureForErrors<AuthenticationError, TooManyRequestsError>(isAssignableTo: true);


        result.ShouldBeFailure<TooManyRequestsError>();

        ClearCache();
    }
}

