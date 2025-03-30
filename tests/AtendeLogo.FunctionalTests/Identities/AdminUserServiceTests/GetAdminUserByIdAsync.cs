namespace AtendeLogo.FunctionalTests.Identities;

public partial class AdminUserServiceTests
{
    [Fact]
    public async Task GetAdminUserByIdAsync_ShouldReturnSuccess()
    {
        // Arrange
        var email = DefaultAdminUserConstants.Email;
        var getEmailResult = await _clientService.GetAdminUserByEmailAsync(email, CancellationToken.None);
        getEmailResult.ShouldBeSuccessful();

        var validUserId = getEmailResult.Value!.Id;

        // Act
        var result = await _clientService.GetAdminUserByIdAsync(validUserId, CancellationToken.None);

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
        var result = await _clientService.GetAdminUserByIdAsync(randomId, CancellationToken.None);

        // Assert
        result.ShouldBeFailure<NotFoundError>();
    }
}

