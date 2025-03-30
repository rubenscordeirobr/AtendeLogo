using System.Net;
using System.Net.Http.Json;

namespace AtendeLogo.FunctionalTests;

public class HttpClientTests : IClassFixture<IdentityWebHostMock<AnonymousRole>>
{
    private readonly HttpClient _httpClient;

    public HttpClientTests(
        IdentityWebHostMock<AnonymousRole> hostFactory,
        ITestOutputHelper testOutput)
    {
        hostFactory.AddTestOutput(testOutput);

        _httpClient = hostFactory.CreateClient();
    }

    [Fact]
    public async Task GetAsync_AnonymousEndpoint_ReturnsOk()
    {
        // Arrange
        var route = $"{RouteConstants.Api}/{RouteConstants.Version}";

        // Act
        var response = await _httpClient.GetAsync(route);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.MediaTypeShouldBeApplicationJson();
    }

    [Fact]
    public async Task GetAsync_NonAnonymousEndpoint_ReturnsUnauthorizedStatusCode()
    {
        // Arrange
        var route = $"{IdentityRouteConstants.Tenants}/{Guid.NewGuid()}";
        
        // Act
        var messageResponse = await _httpClient.GetAsync(route);
        var response = await messageResponse.Content.ReadFromJsonAsync<ErrorResponse>();

        // Assert
        messageResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        messageResponse.MediaTypeShouldBeApplicationJson();

        response.Should().BeOfType<ErrorResponse>();
        response!.Code.Should().Be("HttpRequestExecutor.AnonymousAccessDenied");
    }

    [Fact]
    public async Task GetAsync_NonAnonymousEndpoint_ReturnsBadRequestStatusCode()
    {
        // Arrange
        var route = $"invalid-route/{Guid.NewGuid()}";

        // Act
        var messageResponse = await _httpClient.GetAsync(route);
        var response = await messageResponse.Content.ReadFromJsonAsync<ErrorResponse>();

        // Assert
        messageResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        messageResponse.MediaTypeShouldBeApplicationJson();

        response.Should().BeOfType<ErrorResponse>();
        response!.Code.Should()
            .Be("HttpRequestExecutorFallback.RouteNotRound");
    }
}

public static class HttpResponseMessageExtensions
{
    public static void MediaTypeShouldBeApplicationJson(this HttpResponseMessage response)
    {
        var mediaType = response.Content?.Headers?.ContentType?.MediaType;
        mediaType.Should().Be("application/json");

    }
}

