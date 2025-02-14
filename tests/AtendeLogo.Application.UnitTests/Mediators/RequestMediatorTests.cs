using AtendeLogo.Application.Commands;
using AtendeLogo.Application.Exceptions;
using AtendeLogo.Application.Mediatores;
using AtendeLogo.Common;
using AtendeLogo.Shared.Contracts;
using AtendeLogo.UseCases;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;

namespace AtendeLogo.Application.UnitTests.Mediators;

public class RequestMediatorTests 
{
     
 
    [Fact]
    public async Task RequestMediator_ShouldRunCommandHandler()
    {
        // Arrange
        var mediator = CreateMockRequestMediator();
        var request = new MockCommandRequest { Name = "Test" };

        // Act
        var result = await mediator.RunAsync(request);

        // Assert
        result.IsSuccess
            .Should()
            .BeTrue();
    }

    [Fact]
    public async Task RequestMediator_ShouldReturnValidationFailure()
    {
        // Arrange
        var mediator = CreateMockRequestMediator();
        var request = new MockCommandRequest { Name = "" };

        // Act
        var result = await mediator.RunAsync(request);

        // Assert
        result.IsFailure
            .Should()
            .BeTrue();

        result.Error
            .Should()
            .NotBeNull();

        result.Error
            .Should()
            .BeOfType<ValidationError>();
    }

    [Fact]
    public void RequestMediator_ShouldThrowCommandValidatorNotFoundExeption()
    {
        // Arrange
        var mediator = CreateMockRequestMediator();
        var request = new MockValidatorNotRegistredCommandRequest();

        // Act
        Func<Task> act = async () => await mediator.RunAsync(request);
        // Assert
        act.Should()
            .ThrowAsync<CommandValidatorNotFoundException>();
    }

    [Fact]
    public void RequestMediator_ShouldThrowRequestHandlerNotFoundExeption()
    {
        // Arrange
        var mediator =CreateMockRequestMediator();
        var request = new MockHandlerNotRegistredCommandRequest();
        
        // Act
        Func<Task> act = async () => await mediator.RunAsync(request);
        // Assert
        act.Should()
            .ThrowAsync<RequestHandlerNotFoundException>();

    }

    private IRequestMediator CreateMockRequestMediator()
    {
        var serviceProvider = CreateMockServiceProvider();
        var trackingService = CreateMockTrackingService();
        var loogerMock = new Mock<ILogger<RequestMediator>>().Object;
       
        return new RequestMediator(
            serviceProvider,
            trackingService,
            loogerMock);
    }

    private IServiceProvider CreateMockServiceProvider()
    {
        var types = new Type[]
        {
            typeof(MockCommandHandler),
            typeof(MockCommandRequest),
            typeof(MockCommandRequestValidator),
            typeof(MockHandlerNotRegistredCommandRequest),
            typeof(MockHandlerNotRegistredCommandRequestValidator),
        };

        return new ServiceCollection()
            .AddApplicationHandlersFromTypes(types)
            .AddCommandValidationServicesFromTypes(types)
            .BuildServiceProvider();
    }

    private ICommandTrackingService CreateMockTrackingService()
    {
        var trackingServiceMock = new Mock<ICommandTrackingService>();

        trackingServiceMock
            .Setup(x => x.ExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        trackingServiceMock.Setup(x =>
            x.TrackAsync(It.IsAny<Guid>(),
            It.IsAny<Result<MockResponse>>())
        );

        return trackingServiceMock.Object;
    }

    public record MockResponse : IResponse
    {
    }

    public record MockCommandRequest : ICommandRequest<MockResponse>
    {
        public Guid ClientRequestId { get; }
        public required string Name { get; init; }
    }

    public class MockCommandRequestValidator : AbstractValidator<MockCommandRequest>
    {
        public MockCommandRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty();
        }
    }

    public class MockCommandHandler : CommandHandler<MockCommandRequest, MockResponse>
    {
        protected override Task<Result<MockResponse>> HandleAsync(
            MockCommandRequest command, CancellationToken cancellationToken)
        {
            return Task.FromResult(Result.Success(new MockResponse()));
        }
    }

    public record MockValidatorNotRegistredCommandRequest : ICommandRequest<MockResponse>
    {
        public Guid ClientRequestId => Guid.NewGuid();
    }

    public record MockHandlerNotRegistredCommandRequest : ICommandRequest<MockResponse>
    {
        public Guid ClientRequestId => Guid.NewGuid();
    }

    public class MockHandlerNotRegistredCommandRequestValidator : AbstractValidator<MockHandlerNotRegistredCommandRequest>
    {
        public MockHandlerNotRegistredCommandRequestValidator()
        {
            RuleFor(x => x.ClientRequestId)
                .NotEmpty();
        }
    }
}
