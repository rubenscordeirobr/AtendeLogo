using AtendeLogo.Application.Common;
using AtendeLogo.Application.Contracts.Services;
using AtendeLogo.UseCases.Identities.Authentications.Commands;
using Moq;

namespace AtendeLogo.UseCases.UnitTests.Identities.Authentications.Commands;

public class TenantUserLogoutCommandHandlerTests : IClassFixture<AnonymousServiceProviderMock>
{
    private readonly Mock<IIdentityUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IUserSessionAccessor> _userSessionAccessorMock;
    private readonly Mock<ISessionCacheService> _sessionCacheServiceMock;
    private readonly Mock<IUserSessionRepository> _userSessionRepositoryMock;

    private readonly IServiceProvider _serviceProvider;
    private readonly TenantUserLogoutCommand _command;

    public TenantUserLogoutCommandHandlerTests(AnonymousServiceProviderMock serviceProviderMock,
        ITestOutputHelper testOutput)
    {
        serviceProviderMock.AddTestOutput(testOutput);

        _serviceProvider = serviceProviderMock;
        _command = new TenantUserLogoutCommand
        {
            ClientRequestId = Guid.NewGuid(),
            ClientSessionToken = "test-token"
        };

        _unitOfWorkMock = new Mock<IIdentityUnitOfWork>();
        _userSessionAccessorMock = new Mock<IUserSessionAccessor>();
        _sessionCacheServiceMock = new Mock<ISessionCacheService>();
        _userSessionRepositoryMock = new Mock<IUserSessionRepository>();

        _unitOfWorkMock.Setup(u => u.UserSessions).Returns(_userSessionRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SaveChangesResult(new DomainEventContext([]), 1));
    }

    [Fact]
    public void Handler_ShouldBe_TenantUserLoginCommandHandler()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>() as IRequestMediatorTest;

        // Act
        var handlerType = mediator!.GetRequestHandler(_command);

        // Assert
        handlerType.Should().BeOfType<TenantUserLogoutCommandHandler>();
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnNotFoundFailure_WhenUserSessionNotFound()
    {
        // Arrange
        _userSessionRepositoryMock
            .Setup(repo => repo.GetByClientTokenAsync(_command.ClientSessionToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserSession)null!);

        var handler = new TenantUserLogoutCommandHandler(
            _unitOfWorkMock.Object,
            _userSessionAccessorMock.Object,
            _sessionCacheServiceMock.Object);

       
        // Act
        var result = await handler.RunAsync(_command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
        result.Error?.Message.Should().Contain("UserSession with token");
    }
}

