using AtendeLogo.Application.Services;
using AtendeLogo.Common.Helpers;
using AtendeLogo.Common.Utils;
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
        var expectedSession = new UserSession(
             applicationName: "app",
             clientSessionToken: clientSessionToken,
             ipAddress: "127.0.0.1",
             userAgent: "user-agent",
             language: Language.English,
             authenticationType: AuthenticationType.Credentials,
             userRole: UserRole.Anonymous,
             userType: UserType.Anonymous,
             user_Id: Guid.NewGuid(),
             expirationTime: null,
             tenant_Id: null
        );
         
        var cacheKey = $"user-session:{HashHelper.CreateMd5GuidHash(clientSessionToken)}";

        await cacheRepository.StringSetAsync(cacheKey, JsonUtils.Serialize(expectedSession), TimeSpan.FromHours(1));

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
        var session = new UserSession(
             applicationName: "app",
             clientSessionToken: "test-session-token",
             ipAddress: "127.0.0.1",
             userAgent: "user-agent",
             language: Language.English,
             authenticationType: AuthenticationType.Credentials,
             userRole: UserRole.Anonymous,
             userType: UserType.Anonymous,
             user_Id: Guid.NewGuid(),
             expirationTime: null,
             tenant_Id: null
        );

        session.SetAnonymousSystemSessionId();

        var cacheKey = $"user-session:{HashHelper.CreateMd5GuidHash(session.ClientSessionToken)}";
        // Act
        await service.AddSessionAsync(session);

        // Assert
        var cachedValue = await cacheRepository.StringGetAsync(cacheKey);
        cachedValue.Should().Be(JsonUtils.Serialize(session, options: JsonUtils.CacheJsonSerializerOptions));
    }

    [Fact]
    public async Task AddSessionAsync_ShouldThrowInvalidOperationException_WhenSessionIdIsEmpty()
    {
        // Arrange
        var logger = new LoggerServiceMock<SessionCacheService>();
        var cacheRepository = new CacheRepositoryMock();
        var service = new SessionCacheService(cacheRepository, logger);
        var session = new UserSession(
            applicationName: "app",
            clientSessionToken: "test-session-token",
            ipAddress: "127.0.0.1",
            userAgent: "user-agent",
            language: Language.English,
            authenticationType: AuthenticationType.Credentials,
            userRole: UserRole.Anonymous,
            userType: UserType.Anonymous,
            user_Id: Guid.Empty,
            expirationTime: null,
            tenant_Id: null
         );

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

