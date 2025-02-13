using AtendeLogo.Application.Commands;
using AtendeLogo.Application.Contracts.Events;
using AtendeLogo.Application.Contracts.Handlers;
using AtendeLogo.Application.Exceptions;
using AtendeLogo.Application.Queries;
using AtendeLogo.Common;
using AtendeLogo.Domain.Primitives.Contracts;
using AtendeLogo.Shared.Contracts;
using Microsoft.Extensions.Hosting;

namespace AtendeLogo.Application.UnitTests.Registrars;

public class ApplicationHandlerRegistrarTests
{
    private Type[] GetHandlerTypes()
    {
        return
        [
            typeof(MockCommandHandler),
            typeof(MockSingleQueryHandler),
            typeof(MockCollectionQueryHandler),
            typeof(MockEventHandler)
        ];
    }

    [Fact]
    public void ApplicationHandlerRegistrar_ShouldRegisterHandlersCorrectly()
    {
        // Arrange
        var handlerTypes = GetHandlerTypes();

        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddApplicationHandlersFromTypes(handlerTypes);

        var app = builder.Build();
        var serviceProvider = app.Services;

        // Act
        var commandHandler = serviceProvider.GetService<IRequestHandler<MockCommandRequest, MockResponse>>();
        var singleQueryHandler = serviceProvider.GetService<IRequestHandler<MockSingleQueryRequest, MockResponse>>();
        var collectionQueryHandler = serviceProvider.GetService<IRequestHandler<MockCollectionQueryRequest, MockResponse>>();

        // Assert
        commandHandler.Should().NotBeNull();
        singleQueryHandler.Should().NotBeNull();
        collectionQueryHandler.Should().NotBeNull();

        commandHandler.Should().BeOfType<MockCommandHandler>();
        singleQueryHandler.Should().BeOfType<MockSingleQueryHandler>();
        collectionQueryHandler.Should().BeOfType<MockCollectionQueryHandler>();

        app.Dispose();
    }

    [Fact]
    public void ApplicationHandlerRegistrar_ShouldThrowWhenHandlerAlreadyRegistered()
    {
        // Arrange
        Type[] dupliecatedHandlerTypes = [typeof(MockCommandHandler), typeof(MockCommandHandlerDuplicated)];
        var builder = Host.CreateApplicationBuilder();

        //Act
        Action act = () => builder.Services.AddApplicationHandlersFromTypes(dupliecatedHandlerTypes);

        // Assert
        act.Should().Throw<HandlerRegistrationAlreadyExistsException>();
    }

    public record MockResponse : IResponse
    {

    }
    public record MockCommandRequest : ICommandRequest<MockResponse>
    {
        public Guid ClientRequestId { get; } = Guid.NewGuid();
    }
     
    public record MockSingleQueryRequest : IQueryRequest<MockResponse>
    {
    }

    public record MockCollectionQueryRequest : IQueryRequest<MockResponse>
    {
    }


    public class MockCommandHandler : CommandHandler<MockCommandRequest, MockResponse>
    {
        protected override Task<Result<MockResponse>> HandleAsync(MockCommandRequest command, CancellationToken cancellationToken)
        {
            return Task.FromResult(Result.Success(new MockResponse()));
        }
    }

    public class MockCommandHandlerDuplicated : CommandHandler<MockCommandRequest, MockResponse>
    {
        protected override Task<Result<MockResponse>> HandleAsync(MockCommandRequest command, CancellationToken cancellationToken)
        {
            return Task.FromResult(Result.Success(new MockResponse()));
        }
    }

    public class MockSingleQueryHandler : SingleResultQueryHandler<MockSingleQueryRequest, MockResponse>
    {
        public override Task<Result<MockResponse>> HandleAsync(MockSingleQueryRequest request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Result.Success(new MockResponse()));
        }
    }

    public class MockCollectionQueryHandler : CollectionQueryHandler<MockCollectionQueryRequest, MockResponse>
    {
        public override Task<IReadOnlyList<MockResponse>> HandleAsync(MockCollectionQueryRequest request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyList<MockResponse>>(new List<MockResponse>());
        }
    }

    public record MockDomainEvent : IDomainEvent
    {
    }

    public class MockEventHandler : IDomainEventHandler<MockDomainEvent>
    {
        public Task HandleAsync(MockDomainEvent domainEvent)
        {
            return Task.CompletedTask;
        }
    }
}

