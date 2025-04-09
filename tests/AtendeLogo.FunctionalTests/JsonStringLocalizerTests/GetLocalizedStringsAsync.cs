using AtendeLogo.Shared.Localization;
using AtendeLogo.UseCases.Identities.Authentications.Commands;

namespace AtendeLogo.FunctionalTests;

public partial class JsonStringLocalizerServiceTests
{
    [Fact]
    public async Task HttpClient_GetLocalizedStringsAsync_ByLangTag_ReturnsOk()
    {
        // Arrange
        var route = $"{RouteConstants.StringLocalizerService}/pt";
        // Act
        var messageResponse = await _httpClient.GetAsync(route);
        var response = await messageResponse.Content.ReadFromJsonAsync<LocalizationResourceMap>();

        // Assert
        messageResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        messageResponse.MediaTypeShouldBeApplicationJson();

        response.Should().NotBeNull();
    }

    [Fact]
    public async Task HttpClient_GetLocalizedStringsAsync_ByLangTagAndResourceJey_ReturnsOk()
    {
        // Arrange
        var resourceKey = LocalizationHelper.GetResourceKey<TenantUserLoginCommand>();
        var route = $"{RouteConstants.StringLocalizerService}/pt/{resourceKey}";
        // Act
        var messageResponse = await _httpClient.GetAsync(route);
        var response = await messageResponse.Content.ReadFromJsonAsync<Dictionary<string, string>>();

        // Assert
        messageResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        messageResponse.MediaTypeShouldBeApplicationJson();

        response.Should().NotBeNull();
    }

    [Fact]
    public async Task GetLocalizedStringsAsync_ByLanguage_ShouldBeSuccessful()
    {
        // Act
        var result = await _clientService.GetLocalizationResourceMapAsync(Language.Portuguese);

        // Assert
        result.ShouldBeSuccessful();
    }

    [Fact]
    public async Task DefaultResourceInitializer_ShouldBeSuccessful()
    {
        // Arrange
        var result = await _clientService.GetLocalizationResourceMapAsync(Language.Portuguese);
        var resourceMapInitializer = new DefaultResourceInitializer(_clientService, Language.Portuguese);

        // Act
        result.ShouldBeSuccessful();
        Task act = resourceMapInitializer.InitializeAsync(result.Value!);

        // Assert
        await FluentActions
            .Awaiting(() => act)
            .Should()
            .NotThrowAsync();
    }


    [Fact]
    public async Task GetLocalizedStringsAsync_ByLangTagAndResourceJey_ShouldBeSuccessful()
    {
        // Arrange
        var resourceKey = LocalizationHelper.GetResourceKey<TenantUserLoginCommand>();

        // Act
        var result = await _clientService.GetLocalizedStringsAsync(Language.Portuguese, resourceKey);

        // Assert
        result.ShouldBeSuccessful();
    }
}

