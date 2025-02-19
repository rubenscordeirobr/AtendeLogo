using AtendeLogo.Application.Contracts.Handlers;
using AtendeLogo.Common;
using AtendeLogo.Domain.Primitives.Contracts;

namespace AtendeLogo.Application.UnitTests.Events;

public class DomainEventContextTests  
{
    [Fact]
    public void Constructor_ShouldInitializeEvents()
    {
        // Arrange
        var events = new List<IDomainEvent> { new MockDomainEvent() };

        // Act
        var context = new DomainEventContext(events);

        // Assert
        context.Events.Should().BeEquivalentTo(events);
    }

    [Fact]
    public void Cancel_ShouldSetIsCanceledAndError()
    {
        // Arrange
        var context = new DomainEventContext(new List<IDomainEvent>());
        var error = new DomainEventError("DomainTest.Error", "Test error");

        // Act
        context.Cancel(error);

        // Assert
        context.IsCanceled.Should().BeTrue();
        context.Error.Should().Be(error);
    }

    [Fact]
    public void Cancel_WhenAlreadyLocked_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var context = new DomainEventContext(new List<IDomainEvent>());
        context.LockCancellation();
        var error = new DomainEventError("DomainTest.Error", "Test error");

        // Act
        Action act = () => context.Cancel(error);

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("The context cannot be canceled.");
    }

    [Fact]
    public void GetException_WhenNotCanceled_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var context = new DomainEventContext(new List<IDomainEvent>());

        // Act
        Action act = () => context.GetException();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetException_WhenCanceled_ShouldReturnExceptionWithErrorMessage()
    {
        // Arrange
        var context = new DomainEventContext(new List<IDomainEvent>());
        var error = new DomainEventError("DomainTest.Error", "Test error");
        context.Cancel(error);

        // Act
        var exception = context.GetException();

        // Assert
        exception.Message.Should().Be("Test error");
    }

    [Fact]
    public void AddExecutedEventResults_ShouldAddResults()
    {
        // Arrange
        var context = new DomainEventContext(new List<IDomainEvent>());
        var domainEvent = new MockDomainEvent();
        var results = new List<ExecutedDomainEventResult>
        {
            new ExecutedDomainEventResult(domainEvent, typeof(MockHandler), null, null, null)
        };

        // Act
        context.AddExecutedEventResults(domainEvent, results);

        // Assert
        context.GetExecutedEventResults(domainEvent)
            .Should()
            .BeEquivalentTo(results);
    }

    [Fact]
    public void GetExecutedEventResults_WhenNoResults_ShouldReturnEmptyList()
    {
        // Arrange
        var context = new DomainEventContext(new List<IDomainEvent>());
        var domainEvent = new MockDomainEvent();

        // Act
        var results = context.GetExecutedEventResults(domainEvent);

        // Assert
        results.Should().BeEmpty();
    }
    public class MockDomainEvent : IDomainEvent { }

    public class MockHandler : IApplicationHandler
    {
        public Task HandleAsync(object handlerObject)
            => Task.CompletedTask;
    }
}

