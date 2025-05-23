﻿namespace AtendeLogo.FunctionalTests.Identities;

public partial class TenantValidationServiceTests
{
    [Fact]
    public async Task IsEmailUnique_ShouldReturnTrue()
    {
        // Arrange
        var email = $"{Guid.NewGuid()}@example.com";

        // Act
        var result = await _clientService.IsEmailUniqueAsync(email);

        // Assert
        result.Should()
            .BeTrue();
    }
     
    [Fact]
    public async Task IsEmailUnique_ShouldReturnFalse()
    {
        // Arrange
        var email = SystemTenantConstants.Email;

        // Act
        var result = await _clientService.IsEmailUniqueAsync(email);

        // Assert
        result.Should()
            .BeFalse();
    }

    [Fact]
    public async Task IsEmailUnique_WithTenantAndOwner_ShouldReturnTrue()
    {
        // Arrange
        var email = $"{Guid.NewGuid()}@example.com";

        var currentTenant_Id = SystemTenantConstants.Tenant_Id;
        var currentTenantOwner_Id = SystemTenantConstants.User_Id;

        // Act
        var result = await _clientService.IsEmailUniqueAsync(
            currentTenant_Id,
            currentTenantOwner_Id,
            email);

        // Assert
        result.Should()
            .BeTrue();
    }

    
}

