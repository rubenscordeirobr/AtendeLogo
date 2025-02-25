using AtendeLogo.Presentation.Services;

namespace AtendeLogo.Application.UnitTests.Services;

public class RequestUserSessionServiceTests
{
    [Fact]
    public void GetCurrentSession_ShouldReturnAnonymousSession_WhenNoSessionFoundInContext()
    {
        // Arrange
        var httpContextAccessor = new HttpContextAccessorMock();
        // Ensure Items does not already contain a session.
        httpContextAccessor.HttpContext?.Items.Clear();
        var logger = new LoggerServiceMock<RequestUserSessionService>();
        var service = new RequestUserSessionService(httpContextAccessor, logger);

        // Act
        var session = service.GetCurrentSession();

        // Assert
        session.Should()
            .NotBeNull("An anonymous session should be created when none exists");
    }

    [Fact]
    public void AddClientSessionCookie_ShouldAppendCookieToResponseHeaders()
    {
        // Arrange
        var httpContextAccessor = new HttpContextAccessorMock();
        var logger = new LoggerServiceMock<RequestUserSessionService>();
        var service = new RequestUserSessionService(httpContextAccessor, logger);
        var token = "test-token";

        // Act
        service.AddClientSessionCookie(token);

        // Assert
        // Check that the response headers include the cookie with the expected name and value.
        httpContextAccessor.HttpContext!.Response.Headers.TryGetValue("Set-Cookie", out var setCookieValues);

        setCookieValues.Any(v => v?.Contains("ClientSessionToken=" + token) == true)
            .Should()
            .BeTrue("The cookie should be appended in the response headers");
    }

    [Fact]
    public void GetClientSessionToken_ShouldReturnToken_WhenCookieExists()
    {
        // Arrange

        var httpContextAccessor = new HttpContextAccessorMock();
        // Manually set Cookie header to simulate an incoming cookie.

        var token = "abc123";
        httpContextAccessor.HttpContext!.Request.Headers["Cookie"] = $"ClientSessionToken={token}; otherCookie=otherValue";
      
        var logger = new LoggerServiceMock<RequestUserSessionService>();
        var service = new RequestUserSessionService(httpContextAccessor, logger);

        // Act
        var retrievedToken = service.GetClientSessionToken();

        // Assert
        retrievedToken.Should()
            .Be(token, "The service should retrieve the token from the request cookies");
    }
}
