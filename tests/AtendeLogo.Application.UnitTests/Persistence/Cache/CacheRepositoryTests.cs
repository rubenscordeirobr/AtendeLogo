using AtendeLogo.Infrastructure.Cache;
using Microsoft.Extensions.Logging;
using Moq;
using StackExchange.Redis;

namespace AtendeLogo.Application.UnitTests.Persistence.Cache;

public class CacheRepositoryTests
{
    private readonly Mock<IConnectionMultiplexer> _connectionMock;
    private readonly Mock<IDatabase> _databaseMock;
    private readonly Mock<ILogger<CacheRepository>> _loggerMock;
    private readonly CacheRepository _cacheRepository;

    public CacheRepositoryTests()
    {
        _connectionMock = new Mock<IConnectionMultiplexer>();
        _databaseMock = new Mock<IDatabase>();
        _loggerMock = new Mock<ILogger<CacheRepository>>();

        _connectionMock.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                       .Returns(_databaseMock.Object);

        _cacheRepository = new CacheRepository(_connectionMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task KeyExistsAsync_ShouldReturnTrue_WhenKeyExists()
    {
        // Arrange
        string key = "testKey";
        _databaseMock.Setup(db => db.KeyExistsAsync(key, It.IsAny<CommandFlags>()))
                     .ReturnsAsync(true);

        // Act
        bool result = await _cacheRepository.KeyExistsAsync(key);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task KeyExistsAsync_ShouldReturnFalse_WhenExceptionThrown()
    {
        // Arrange
        string key = "testKey";
        _databaseMock.Setup(db => db.KeyExistsAsync(key, It.IsAny<CommandFlags>()))
                     .ThrowsAsync(new Exception("Redis error"));

        // Act
        bool result = await _cacheRepository.KeyExistsAsync(key);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task StringGetAsync_ShouldReturnString_WhenValueExists()
    {
        // Arrange
        string key = "testKey";
        string expectedValue = "value";
        RedisValue redisValue = expectedValue;
        _databaseMock.Setup(db => db.StringGetAsync(key, It.IsAny<CommandFlags>()))
                     .ReturnsAsync(redisValue);

        // Act
        var result = await _cacheRepository.StringGetAsync(key);

        // Assert
        result.Should().Be(expectedValue);
    }

    [Fact]
    public async Task StringGetAsync_ShouldReturnNull_WhenValueDoesNotExist()
    {
        // Arrange
        string key = "testKey";
        RedisValue redisValue = RedisValue.Null;
        _databaseMock.Setup(db => db.StringGetAsync(key, It.IsAny<CommandFlags>()))
                     .ReturnsAsync(redisValue);

        // Act
        var result = await _cacheRepository.StringGetAsync(key);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task StringGetAsync_ShouldReturnNull_WhenExceptionThrown()
    {
        // Arrange
        string key = "testKey";
        _databaseMock.Setup(db => db.StringGetAsync(key, It.IsAny<CommandFlags>()))
                     .ThrowsAsync(new Exception("Redis error"));

        // Act
        var result = await _cacheRepository.StringGetAsync(key);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task KeyDeleteAsync_ShouldCallDeleteMethod()
    {
        // Arrange
        string key = "testKey";
        _databaseMock.Setup(db => db.KeyDeleteAsync(key, It.IsAny<CommandFlags>()))
                     .ReturnsAsync(true);

        // Act
        await _cacheRepository.KeyDeleteAsync(key);

        // Assert
        _databaseMock.Verify(db => db.KeyDeleteAsync(key, It.IsAny<CommandFlags>()), Times.Once);
    }

    [Fact]
    public async Task KeyDeleteAsync_ShouldNotTrowException_WhenDatabaseExceptionThrown()
    {
        // Arrange
        string key = "testKey";
        _databaseMock.Setup(db => db.KeyDeleteAsync(key, It.IsAny<CommandFlags>()))
                     .ThrowsAsync(new Exception("Redis error"));

        // Act 
        Func<Task> act = async () => await _cacheRepository.KeyDeleteAsync(key);

        // Assert
        await act.Should()
            .NotThrowAsync<Exception>();
    }
     
    [Fact]
    public async Task StringSetAsync_ShouldNotThrowException_WhenDatabaseExceptionThrown()
    {
        // Arrange
        string key = "testKey";
        string value = "value";
        TimeSpan expiry = TimeSpan.FromMinutes(5);
        _databaseMock.Setup(db => db.StringSetAsync(key, value, expiry, It.IsAny<When>(), It.IsAny<CommandFlags>()))
                     .ThrowsAsync(new Exception("Redis error"));

        // Act
        Func<Task> act = async () => await _cacheRepository.StringSetAsync(key, value, expiry);

        // Assert
        await act.Should()
            .NotThrowAsync<Exception>();
    }
}

