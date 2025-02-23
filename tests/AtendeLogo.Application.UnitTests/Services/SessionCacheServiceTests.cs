﻿using System.Text.Json;
using AtendeLogo.Common.Helpers;
using AtendeLogo.Infrastructure.Cache;
using AtendeLogo.Persistence.Identity.Extensions;

namespace AtendeLogo.Application.UnitTests.Services;

public class SessionCacheServiceTests
{
    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenClientSessionTokenExists()
    {
        // Arrange
        var logger = new LoggerServiceMock<SessionCacheService>();
        var cacheRepository = new CacheRepositoryMock();
        var service = new SessionCacheService(cacheRepository, logger);

        var clientSessionToken = "test-session-token";
        var cacheKey = $"user-session:{HashHelper.CreateMd5GuidHash(clientSessionToken)}";

        await cacheRepository.StringSetAsync(cacheKey, "value", TimeSpan.FromHours(1));

        // Act
        var result = await service.ExistsAsync(clientSessionToken);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnFalse_WhenClientSessionTokenDoesNotExist()
    {
        // Arrange
        var logger = new LoggerServiceMock<SessionCacheService>();
        var cacheRepository = new CacheRepositoryMock();
        var service = new SessionCacheService(cacheRepository, logger);
        var clientSessionToken = "test-session-token";

        // Act
        var result = await service.ExistsAsync(clientSessionToken);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task GetSessionAsync_ShouldReturnSession_WhenClientSessionTokenExists()
    {
        // Arrange
        var logger = new LoggerServiceMock<SessionCacheService>();
        var cacheRepository = new CacheRepositoryMock();
        var service = new SessionCacheService(cacheRepository, logger);
        var clientSessionToken = "test-session-token";
        var expectedSession = new UserSession("app", clientSessionToken, "127.0.0.1", "user-agent", null, Language.English, AuthenticationType.Email_Password, Guid.NewGuid(), null);

        var cacheKey = $"user-session:{HashHelper.CreateMd5GuidHash(clientSessionToken)}";

        await cacheRepository.StringSetAsync(cacheKey, JsonSerializer.Serialize(expectedSession), TimeSpan.FromHours(1));

        // Act
        var result = await service.GetSessionAsync(clientSessionToken);

        // Assert
        result.Should().NotBeNull();
        result!.ClientSessionToken.Should().Be(expectedSession.ClientSessionToken);
    }

    [Fact]
    public async Task GetSessionAsync_ShouldReturnNull_WhenClientSessionTokenDoesNotExist()
    {
        // Arrange
        var logger = new LoggerServiceMock<SessionCacheService>();
        var cacheRepository = new CacheRepositoryMock();
        var service = new SessionCacheService(cacheRepository, logger);
        var clientSessionToken = "test-session-token";

        // Act
        var result = await service.GetSessionAsync(clientSessionToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddSessionAsync_ShouldAddSessionToCache_WhenSessionIsValid()
    {
        // Arrange
        var logger = new LoggerServiceMock<SessionCacheService>();
        var cacheRepository = new CacheRepositoryMock();
        var service = new SessionCacheService(cacheRepository, logger);
        var session = new UserSession("app", "test-session-token", "127.0.0.1", "user-agent", null, Language.English, AuthenticationType.Email_Password, Guid.NewGuid(), null);
        session.SetAnonymousSystemSessionId();

        var cacheKey = $"user-session:{HashHelper.CreateMd5GuidHash(session.ClientSessionToken)}";
        // Act
        await service.AddSessionAsync(session);

        // Assert
        var cachedValue = await cacheRepository.StringGetAsync(cacheKey);
        cachedValue.Should().Be(JsonSerializer.Serialize(session));
    }

    [Fact]
    public async Task AddSessionAsync_ShouldThrowInvalidOperationException_WhenSessionIdIsEmpty()
    {
        // Arrange
        var logger = new LoggerServiceMock<SessionCacheService>();
        var cacheRepository = new CacheRepositoryMock();
        var service = new SessionCacheService(cacheRepository, logger);
        var session = new UserSession("app", "test-session-token", "127.0.0.1", "user-agent", null, Language.English, AuthenticationType.Email_Password, Guid.Empty, null);

        // Act
        Func<Task> act = async () => await service.AddSessionAsync(session);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Cannot add session with empty Id");
    }

    [Fact]
    public async Task RemoveSessionAsync_ShouldRemoveSessionFromCache_WhenClientSessionTokenExists()
    {
        // Arrange
        var logger = new LoggerServiceMock<SessionCacheService>();
        var cacheRepository = new CacheRepositoryMock();
        var service = new SessionCacheService(cacheRepository, logger);
        var clientSessionToken = "test-session-token";
        var cacheKey = $"user-session:{HashHelper.CreateMd5GuidHash(clientSessionToken)}";

        await cacheRepository.StringSetAsync(cacheKey, "value", TimeSpan.FromHours(1));

        // Act
        await service.RemoveSessionAsync(clientSessionToken);

        // Assert
        var cachedValue = await cacheRepository.StringGetAsync(cacheKey);
        cachedValue.Should().BeNull();
    }
}

