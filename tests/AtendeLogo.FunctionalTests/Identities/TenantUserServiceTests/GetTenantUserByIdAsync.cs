namespace AtendeLogo.FunctionalTests.Identities;

public partial class TenantUserServiceTests
{
    [Fact]
    public async Task GetAdminUserByIdAsync_ShouldReturnSuccess()
    {
        // Arrange
        var email = SystemTenantConstants.Email;
        var getEmailResult = await _clientService.GetTenantUserByEmailAsync(email, CancellationToken.None);
        getEmailResult.ShouldBeSuccessful();

        var validUserId = getEmailResult.Value!.Id;

        // Act
        var result = await _clientService.GetTenantUserByIdAsync(validUserId, CancellationToken.None);

        // Assert
        result.ShouldBeSuccessful();
        result.Value!   .Id.Should().Be(validUserId);
    }

    [Fact]
    public async Task GetAdminUserByIdAsync_ShouldReturnFailure()
    {
        // Arrange
        var randomId = Guid.NewGuid();

        // Act
        var result = await _clientService.GetTenantUserByIdAsync(randomId, CancellationToken.None);

        // Assert
        result.ShouldBeFailure<NotFoundError>();
    }
}

