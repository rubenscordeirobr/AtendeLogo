﻿using AtendeLogo.UseCases.Identities.Authentications.Commands;
using AtendeLogo.UseCases.UnitTests.TestSupport;

namespace AtendeLogo.UseCases.UnitTests.Identities.Authentications.Commands;

public class TenantUserLoginCommandHandlerTests : IClassFixture<AnonymousServiceProviderMock>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TenantUserLoginCommand _validCommand;

    public TenantUserLoginCommandHandlerTests(AnonymousServiceProviderMock serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _validCommand = new TenantUserLoginCommand
        {
            ClientRequestId = Guid.NewGuid(),
            EmailOrPhoneNumber = SystemTenantConstants.TenantSystemEmail,
            Password = "TenantAdmin@Teste%#",
            RememberMe = true
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
    public async Task HandleAsync_ReturnsFailure_WhenUserNotFound()
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
        result.ShouldNotBeSuccessful();
        result.Error!.Message.Should().Contain("does not exist");
    }

    [Fact]
    public async Task HandleAsync_ReturnsFailure_WhenPasswordInvalid()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>();
        var command = _validCommand with { Password = "abc" };

        // Act
        var result = await mediator.TestRunAsync(command);

        // Assert
        result.ShouldNotBeSuccessful();
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public async Task HandleAsync_ReturnsFailure_WhenPasswordIsInvalid()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IRequestMediator>();
        var command = _validCommand with { Password = "InvalidPassword" };

        // Act
        var result = await mediator.TestRunAsync(command);

        // Assert
        result.ShouldNotBeSuccessful();
        result.Error!.Message.Should().Contain("Invalid credentials");
    }
}

