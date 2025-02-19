namespace AtendeLogo.Application.UnitTests.Services;

public class UserSessionVerificationServiceTests
{
    [Fact]
    public async Task Verify_ShouldNotThrowException()
    {
        // Arrange
        var serviceProvider = new AnonymousServiceProviderMock();
        var registrationService = serviceProvider.GetRequiredService<IUserSessionVerificationService>();

        //Act
        Func<Task> act = async () => await registrationService.VerifyAsync();

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Verify_ShouldCreateNewUserSession()
    {
        // Arrange
        var serviceProvider = new AnonymousServiceProviderMock();
        var registrationService = serviceProvider.GetRequiredService<IUserSessionVerificationService>();

        //Act
        var userSession = await registrationService.VerifyAsync();

        // Assert
        userSession.Should().NotBeNull();
        userSession.Should().BeOfType<UserSession>();

        userSession.ClientSessionToken
            .Should()
            .NotBeNullOrWhiteSpace();

        userSession.StartedAt
            .Should()
            .BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }
}

