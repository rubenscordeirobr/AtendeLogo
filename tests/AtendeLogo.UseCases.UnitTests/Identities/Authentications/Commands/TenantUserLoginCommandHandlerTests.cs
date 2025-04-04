﻿using AtendeLogo.Application.Abstractions.Services;
using AtendeLogo.Domain.Entities.Identities.Events;
using AtendeLogo.UseCases.Identities.Authentications.Commands;

namespace AtendeLogo.UseCases.UnitTests.Identities.Authentications.Commands;

public class TenantUserLoginCommandHandlerTests : IClassFixture<ServiceProviderMock<AnonymousRole>>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TenantUserLoginCommand _validCommand;

    public TenantUserLoginCommandHandlerTests(ServiceProviderMock<AnonymousRole> serviceProviderMock,
        ITestOutputHelper testOutput)
    {
        serviceProviderMock.AddTestOutput(testOutput);

        _serviceProvider = serviceProviderMock;
        _validCommand = new TenantUserLoginCommand
        {
            EmailOrPhoneNumber = SystemTenantConstants.Email,
            Password = SystemTenantConstants.TestPassword,
            KeepSession = true
        };
    }

    [Fact]
    public void Handler_ShouldBe_TenantUserLoginCommandHandler()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>() as IRequestMediatorTest;

        // Act
        var handlerType = mediator!.GetRequestHandler(_validCommand);

        // Assert
        handlerType.Should().BeOfType<TenantUserLoginCommandHandler>();
    }

    [Fact]
    public async Task HandleAsync_ShouldBeSuccessful_WhenUserNotFound()
    {

        await using (var scope = _serviceProvider.CreateAsyncScope())
        {
            // Arrange
            var mediator = scope.ServiceProvider.GetRequiredService<IRequestMediator>();
            var eventMediator = (IEventMediatorTest)scope.ServiceProvider.GetRequiredService<IEventMediator>();
            var cacheSessionService = scope.ServiceProvider.GetRequiredService<IUserSessionCacheService>();

            eventMediator.CapturedEvents
                .Should()
                .BeEmpty();

            // Act
            var result = await mediator.RunAsync(_validCommand);

            // Assert
            result.ShouldBeSuccessful();

            // Check if the events were captured
            eventMediator.CapturedEvents
                .Should().NotBeEmpty()
                .And.Contain(@event => @event is TenantUserSessionStartedEvent)
                .And.Contain(@event => @event is UserSessionStartedEvent);

            eventMediator.ExecutedDomainEvents
                .Should().NotBeEmpty()
                .And.Contain(result => result.DomainEvent is UserLoggedInEvent &&
                                       result.HandlerType == typeof(UserLoggedInEventHandler));
                 
            var userSession = eventMediator.CapturedEvents
                .OfType<TenantUserSessionStartedEvent>()
                .First()
                .UserSession;

            userSession.CreatedAt
                .Should()
                .BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(30));

            var response = result.Value!;

            // Check if the session was cached
            var cachedUseSession = await cacheSessionService.GetSessionAsync(response.UserSession.Id);
            cachedUseSession
                .Should()
                .NotBeNull();

            // Check if the session is the same as the one created
            cachedUseSession!.Id
                .Should().Be(userSession.Id);

        }
    }

    [Fact]
    public async Task HandleAsync_ShouldBeFailure_WhenUserNotFound()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>();
        // Simulate user not found by using a non-existent email/phone number.
        var command = _validCommand with
        {
            EmailOrPhoneNumber = "notfound@atendelogo.com"
        };

        // Act
        var result = await mediator.TestRunAsync(command);

        // Assert
        result.ShouldBeFailure();
        result.Error!.Message.Should().Contain("does not exist");
    }

    [Fact]
    public async Task HandleAsync_ShouldBeFailure_WhenPasswordInvalid()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>();
        var command = _validCommand with { Password = "abc" };

        // Act
        var result = await mediator.TestRunAsync(command);

        // Assert
        result.ShouldBeFailure();
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public async Task HandleAsync_ShouldBeFailure_WhenPasswordIsInvalid()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>();
        var command = _validCommand with { Password = "InvalidPassword" };

        // Act
        var result = await mediator.TestRunAsync(command);

        // Assert
        result.ShouldBeFailure();
        result.Error!.Message.Should().Contain("Invalid password");
    }
}

