﻿using AtendeLogo.Shared.Localization;
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
    public async Task GetLocalizedStringsAsync_ByCulture_ShouldBeSuccessful()
    {
        // Act
        var result = await _clientService.GetLocalizationResourceMapAsync(Culture.PtBr);

        // Assert
        result.ShouldBeSuccessful();
    }

    [Fact]
    public async Task DefaultResourceInitializer_ShouldBeSuccessful()
    {
        // Arrange
        var result = await _clientService.GetLocalizationResourceMapAsync(Culture.PtBr);
        var resourceMapInitializer = new DefaultResourceInitializer(_clientService, Culture.PtBr);

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
    public async Task GetLocalizedStringsAsync_ByCultureResourceJey_ShouldBeSuccessful()
    {
        // Arrange
        var resourceKey = LocalizationHelper.GetResourceKey<TenantUserLoginCommand>();

        // Act
        var result = await _clientService.GetLocalizedStringsAsync(Culture.PtBr, resourceKey);

        // Assert
        result.ShouldBeSuccessful();
    }
}

