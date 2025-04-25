namespace AtendeLogo.Common.UnitTests.Utils;

public class UriAvailabilityUtilsTests
{
    [Theory]
    [InlineData(new[] { "https://facebook.com", "https://unavailable1.example" }, "https://facebook.com")]
    [InlineData(new[] { "https://unavailable1.example", "https://google.com" }, "https://google.com")]
    public async Task GetFirstAvailableBaseUrlAsync_WithMixedUrls_ReturnsFirstAvailableUrl(string[] urls, string expected)
    {
        // Arrange
        Uri CheckUriBuilder(Uri baseUri) => baseUri;
        
        // Act
        var result = await UriAvailabilityUtils.GetFirstAvailableBaseUrlAsync(
            urls,
            CheckUriBuilder,
            timeout: TimeSpan.FromSeconds(5));
        
        // Assert
        result.Should().Be(expected);
    }
    
    [Fact]
    public async Task GetFirstAvailableBaseUrlAsync_WithNoAvailableUrls_ThrowsInvalidOperationException()
    {
        // Arrange
        var urls = new[] { "https://unavailable1.example", "https://unavailable2.example" };
        Uri CheckUriBuilder(Uri baseUri) => baseUri;
        
        // Act
        Func<Task> act = async () => await UriAvailabilityUtils.GetFirstAvailableBaseUrlAsync(
            urls,
            CheckUriBuilder,
            timeout: TimeSpan.FromSeconds(1));
        
        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("No reachable base URL found in the provided list.");
    }
    
    [Theory]
    [InlineData(new[] { "invalid-uri", "https://valid.com" }, "https://valid.com")]
    [InlineData(new[] { "not-a-url", "also-not-a-url", "https://example.com" }, "https://example.com")]
    public async Task GetFirstAvailableBaseUrlAsync_WithInvalidUrls_SkipsInvalidAndContinues(string[] urls, string expected)
    {
        // Arrange
        Uri CheckUriBuilder(Uri baseUri) => baseUri;
        
        // Act
        var result = await UriAvailabilityUtils.GetFirstAvailableBaseUrlAsync(
            urls,
            CheckUriBuilder,
            timeout: TimeSpan.FromSeconds(1));
        
        // Assert
        result.Should().Be(expected);
    }
    
    [Fact]
    public async Task GetFirstAvailableBaseUrlAsync_WithCustomCheckUriBuilder_UsesProvidedBuilder()
    {
        // Arrange
        var urls = new[] { "https://example.com" };
        var customBuilderCalled = false;
        
        Uri CustomUriBuilder(Uri baseUri)
        {
            customBuilderCalled = true;
            return baseUri;
        }
        
        // Act
        var result = await UriAvailabilityUtils.GetFirstAvailableBaseUrlAsync(
            urls,
            CustomUriBuilder,
            timeout: TimeSpan.FromSeconds(1));
        
        // Assert
        customBuilderCalled.Should().BeTrue();
        result.Should().Be("https://example.com");
    }
    
    [Fact]
    public async Task GetFirstAvailableBaseUrlAsync_WhenAllUrlsTimeout_ThrowsInvalidOperationException()
    {
        // Arrange
        var urls = new[] { "https://veryslow.example", "https://alsoslow.example" };
        Uri CheckUriBuilder(Uri baseUri) => baseUri;
        
        // Act
        Func<Task> act = async () => await UriAvailabilityUtils.GetFirstAvailableBaseUrlAsync(
            urls,
            CheckUriBuilder,
            timeout: TimeSpan.FromMilliseconds(1)); // Very short timeout to force timeout
        
        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("No reachable base URL found in the provided list.");
    }
    
    [Fact]
    public async Task GetFirstAvailableBaseUrlAsync_WithNullCheckUriBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        var urls = new[] { "https://example.com" };
        Func<Uri, Uri> checkUriBuilder = null!;
        
        // Act
        Func<Task> act = async () => await UriAvailabilityUtils.GetFirstAvailableBaseUrlAsync(
            urls,
            checkUriBuilder,
            timeout: TimeSpan.FromSeconds(1));
        
        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
    
    [Fact]
    public async Task GetFirstAvailableBaseUrlAsync_WithEmptyUrlList_ThrowsInvalidOperationException()
    {
        // Arrange
        var urls = Array.Empty<string>();
        Uri CheckUriBuilder(Uri baseUri) => baseUri;
        
        // Act
        Func<Task> act = async () => await UriAvailabilityUtils.GetFirstAvailableBaseUrlAsync(
            urls,
            CheckUriBuilder,
            timeout: TimeSpan.FromSeconds(1));
        
        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("No reachable base URL found in the provided list.");
    }
}
