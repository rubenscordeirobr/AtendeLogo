using AtendeLogo.Application.Commands;
using AtendeLogo.Application.Exceptions;
using AtendeLogo.Application.Mediatores;
using AtendeLogo.Common;
using AtendeLogo.Shared.Contracts;
using AtendeLogo.UseCases;
using FluentValidation;
using Microsoft.Extensions.Hosting;
using Moq;

namespace AtendeLogo.Application.UnitTests.Mediators;

public class RequestMediatorTests : IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IHost _app;
    private readonly ICommandTrackingService _trackingService;

    public RequestMediatorTests()
    {
        (_serviceProvider, _app) = CreateMockServiceProvider();
        _trackingService = CreateMockTrackingService();

    }

    private (IServiceProvider, IHost _app) CreateMockServiceProvider()
    {
        var builder = Host.CreateApplicationBuilder();
        var types = new Type[]
        {
            typeof(MockCommandHandler),
            typeof(MockCommandRequest),
            typeof(MockCommandRequestValidator),
            typeof(MockHandlerNotRegistredCommandRequest),
            typeof(MockHandlerNotRegistredCommandRequestValidator),
        };

        builder.Services.AddApplicationHandlersFromTypes(types);
        builder.Services.AddCommandValidationServicesFromTypes(types);

        var app = builder.Build();

        var serviceProvider = app.Services;
        return (serviceProvider, app);
    }

    [Fact]
    public async Task RequestMediator_ShouldRunCommandHandler()
    {
        // Arrange
        var request = new MockCommandRequest { Name = "Test" };

        var mediator = new RequestMediator(
            _serviceProvider,
            _trackingService);

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
        var request = new MockCommandRequest { Name = "" };
        var mediator = new RequestMediator(
            _serviceProvider,
            _trackingService);
        
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
        var request = new MockValidatorNotRegistredCommandRequest();
        var mediator = new RequestMediator(
            _serviceProvider,
            _trackingService);

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
        var request = new MockHandlerNotRegistredCommandRequest();
        var mediator = new RequestMediator(
            _serviceProvider,
            _trackingService);

        // Act
        Func<Task> act = async () => await mediator.RunAsync(request);
        // Assert
        act.Should()
            .ThrowAsync<RequestHandlerNotFoundException>();

    }

    public void Dispose()
    {
        _app?.Dispose();
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
