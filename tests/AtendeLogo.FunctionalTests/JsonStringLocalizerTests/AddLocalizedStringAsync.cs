namespace AtendeLogo.FunctionalTests;

public partial class JsonStringLocalizerServiceTests
{
    [Fact]
    public async Task AddLocalizedStringAsync_ShouldBeSuccessful()
    {
        // Arrange
        var language = Language.Portuguese;
        var resourceKey = "test-resource-key";
        var localizationKey = "TestLocalizationKey";
        var defaultValue = "TestValue";
        // Act
        var result = await _clientService.AddLocalizedStringAsync(
            language,
            resourceKey,
            localizationKey,
            defaultValue);
        // Assert

        result.ShouldBeSuccessful();
    }

    [Fact]
    public async Task UpdateDefaultLocalizedStringAsync_ShouldBeSuccessful()
    {
        // Arrange
        var resourceKey = "test-resource-key";
        var localizationKey = "TestLocalizationKey";
        var defaultValue = "TestValue";
        // Act
        var result = await _clientService.UpdateDefaultLocalizedStringAsync(
            resourceKey,
            localizationKey,
            defaultValue);
        // Assert

        result.ShouldBeSuccessful();
    }

}

