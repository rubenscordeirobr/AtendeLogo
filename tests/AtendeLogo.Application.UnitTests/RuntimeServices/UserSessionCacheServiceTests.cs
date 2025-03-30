using AtendeLogo.Common.Infos;
using AtendeLogo.Domain.Entities.Identities.Factories;
using AtendeLogo.Persistence.Identity.Extensions;
using AtendeLogo.Shared.Models.Identities;

namespace AtendeLogo.Application.UnitTests.RuntimeServices;

public class UserSessionCacheServiceTests
{
    private readonly ITestOutputHelper _testOutput;

    public UserSessionCacheServiceTests(ITestOutputHelper testOutput)
    {
        _testOutput = testOutput;
    }


    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenClientSessionTokenExists()
    {
        // Arrange
        var logger = new TestOutputLogger<UserSessionCacheService>(_testOutput);
        var cacheRepository = new CacheRepositoryMock();
        var service = new UserSessionCacheService(cacheRepository, logger);

        var clientSessionToken = Guid.NewGuid();
        var cacheKey = $"user-session:{clientSessionToken}";

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
        var logger = new TestOutputLogger<UserSessionCacheService>(_testOutput);
        var cacheRepository = new CacheRepositoryMock();
        var service = new UserSessionCacheService(cacheRepository, logger);
       
        // Act
        var result = await service.ExistsAsync(Guid.NewGuid());

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task GetSessionAsync_ShouldReturnSession_WhenClientSessionTokenExists()
    {
        // Arrange
        var logger = new TestOutputLogger<UserSessionCacheService>(_testOutput);
        var cacheRepository = new CacheRepositoryMock();
        var service = new UserSessionCacheService(cacheRepository, logger);

        var session_Id = Guid.NewGuid();
        var headerInfo = ClientRequestHeaderInfo.System;
        var user = SystemTenantConstants.OwnerUser;
        var tenant_Id = SystemTenantConstants.Tenant_Id;

        var expectedSession = UserSessionFactory.Create(
            user,
            headerInfo,
            AuthenticationType.Anonymous,
            keepSession: true,
            tenant_id: tenant_Id);

        expectedSession.SetPropertyValue(p => p.Id, session_Id);

     
        var cacheKey = $"user-session:{session_Id}";
        var json = JsonUtils.Serialize(expectedSession, JsonUtils.CacheJsonSerializerOptions);
        await cacheRepository.StringSetAsync(cacheKey, json, TimeSpan.FromHours(1));

        // Act
        var result = await service.GetSessionAsync(session_Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(session_Id);
        result.User_Id.Should().Be(user.Id);
        result.Tenant_Id.Should().Be(tenant_Id);
    }

    [Fact]
    public async Task GetSessionAsync_ShouldReturnNull_WhenClientSessionTokenDoesNotExist()
    {
        // Arrange
        var logger = new TestOutputLogger<UserSessionCacheService>(_testOutput);
        var cacheRepository = new CacheRepositoryMock();
        var service = new UserSessionCacheService(cacheRepository, logger);
        var senssionId = Guid.NewGuid();

        // Act
        var result = await service.GetSessionAsync(senssionId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddSessionAsync_ShouldAddSessionToCache_WhenSessionIsValid()
    {
        // Arrange
        var logger = new TestOutputLogger<UserSessionCacheService>(_testOutput);
        var cacheRepository = new CacheRepositoryMock();
        var service = new UserSessionCacheService(cacheRepository, logger);
         
        var headerInfo = ClientRequestHeaderInfo.System;
        var anonymousUser = AnonymousUserConstants.AnonymousUser;
        var session = UserSessionFactory.Create(
            anonymousUser,
            headerInfo,
            AuthenticationType.Anonymous,
            keepSession: true,
            tenant_id: null);
             

        session.SetAnonymousSystemSessionId();

        var expected = JsonUtils.Serialize(CachedUserSession.Create(session), options: JsonUtils.CacheJsonSerializerOptions);
        var cacheKey = $"user-session:{session.Id}";

        // Act
        await service.AddSessionAsync(session);

        // Assert
        var cachedValue = await cacheRepository.StringGetAsync(cacheKey);

        cachedValue.Should().Be(expected);
    }

    [Fact]
    public async Task AddSessionAsync_ShouldThrowInvalidOperationException_WhenSessionIdIsEmpty()
    {
        // Arrange
        var logger = new TestOutputLogger<UserSessionCacheService>(_testOutput);
        var cacheRepository = new CacheRepositoryMock();
        var service = new UserSessionCacheService(cacheRepository, logger);

        var headerInfo = ClientRequestHeaderInfo.System;
        var anonymousUser = AnonymousUserConstants.AnonymousUser;
        var session = UserSessionFactory.Create(
            anonymousUser,
            headerInfo,
            AuthenticationType.Anonymous,
            keepSession: true,
            tenant_id: null);

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
        var logger = new TestOutputLogger<UserSessionCacheService>(_testOutput);
        var cacheRepository = new CacheRepositoryMock();
        var service = new UserSessionCacheService(cacheRepository, logger);
        var clientSessionToken = Guid.NewGuid();
        var cacheKey = $"user-session:{clientSessionToken}";

        await cacheRepository.StringSetAsync(cacheKey, "value", TimeSpan.FromHours(1));

        // Act
        await service.RemoveSessionAsync(clientSessionToken);

        // Assert
        var cachedValue = await cacheRepository.StringGetAsync(cacheKey);
        cachedValue.Should()
            .BeNull();
    }
}

