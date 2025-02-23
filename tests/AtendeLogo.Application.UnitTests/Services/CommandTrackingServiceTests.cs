using AtendeLogo.Common;
using AtendeLogo.Infrastructure.Cache;

namespace AtendeLogo.Application.UnitTests.Services;

public class CommandTrackingServiceTests
{
    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenClientRequestIdExists()
    {
        // Arrange
        var logger = new LoggerServiceMock<CommandTrackingService>();
        var cacheRepository = new CacheRepositoryMock();
        var service = new CommandTrackingService(cacheRepository, logger);
        var clientRequestId = Guid.NewGuid();
        await cacheRepository.StringSetAsync($"request-tracker:{clientRequestId}", "value", TimeSpan.FromMinutes(5));

        // Act
        var result = await service.ExistsAsync(clientRequestId, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnFalse_WhenClientRequestIdDoesNotExist()
    {
        // Arrange
        var logger = new LoggerServiceMock<CommandTrackingService>();
        var cacheRepository = new CacheRepositoryMock();
        var service = new CommandTrackingService(cacheRepository, logger);
        var clientRequestId = Guid.NewGuid();

        // Act
        var result = await service.ExistsAsync(clientRequestId, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task TryGetResultAsync_ShouldReturnResult_WhenClientRequestIdExists()
    {
        // Arrange
        var logger = new LoggerServiceMock<CommandTrackingService>();
        var cacheRepository = new CacheRepositoryMock();
        var service = new CommandTrackingService(cacheRepository, logger);
       
        var clientRequestId = Guid.NewGuid();
        var expectedResult = Result.Success("value");

        await cacheRepository.StringSetAsync($"request-tracker:{clientRequestId}", "\"value\"", TimeSpan.FromMinutes(5));

        // Act
        var result = await service.TryGetResultAsync<string>(clientRequestId, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Value.Should().Be(expectedResult.Value);
    }

    [Fact]
    public async Task TryGetResultAsync_ShouldReturnNull_WhenClientRequestIdDoesNotExist()
    {
        // Arrange
        var logger = new LoggerServiceMock<CommandTrackingService>();
        var cacheRepository = new CacheRepositoryMock();
        var service = new CommandTrackingService(cacheRepository, logger);
        var clientRequestId = Guid.NewGuid();

        // Act
        var result = await service.TryGetResultAsync<string>(clientRequestId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task TrackAsync_ShouldAddResultToCache_WhenResultIsSuccess()
    {
        // Arrange
        var logger = new LoggerServiceMock<CommandTrackingService>();
        var cacheRepository = new CacheRepositoryMock();
        var service = new CommandTrackingService(cacheRepository, logger);
        var clientRequestId = Guid.NewGuid();
        var result = Result.Success("value");

        // Act
        await service.TrackAsync(clientRequestId, result);

        // Assert
        var cachedValue = await cacheRepository.StringGetAsync($"request-tracker:{clientRequestId}");
        cachedValue.Should().Be("\"value\"");
    }

    [Fact]
    public async Task TrackAsync_ShouldThrowInvalidOperationException_WhenResultIsFailure()
    {
        // Arrange
        var logger = new LoggerServiceMock<CommandTrackingService>();
        var cacheRepository = new CacheRepositoryMock();
        var service = new CommandTrackingService(cacheRepository, logger);
        var clientRequestId = Guid.NewGuid();
        var result = Result.Failure<string>(new ValidationError("error_code", "error_message"));

        // Act
        Func<Task> act = async () => await service.TrackAsync(clientRequestId, result);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Only successful results can be tracked.");
    }
}
