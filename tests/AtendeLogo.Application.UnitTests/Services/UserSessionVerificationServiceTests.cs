﻿namespace AtendeLogo.Application.UnitTests.Services;

public class UserSessionVerificationServiceTests :IClassFixture<AnonymousServiceProviderMock>
{
    private readonly IServiceProvider _serviceProvider;

    public UserSessionVerificationServiceTests(AnonymousServiceProviderMock serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
     
    [Fact]
    public async Task Verify_ShouldNotThrowException()
    {
        // Arrange
        var registrationService = _serviceProvider.GetRequiredService<IUserSessionVerificationService>();

        //Act
        Func<Task> act = registrationService.VerifyAsync;

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Verify_ShouldCreateNewUserSession()
    {
        // Arrange
        var registrationService = _serviceProvider.GetRequiredService<IUserSessionVerificationService>();

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

