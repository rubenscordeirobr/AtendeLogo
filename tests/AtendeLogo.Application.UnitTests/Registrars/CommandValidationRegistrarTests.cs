using AtendeLogo.Shared.Contracts;
using AtendeLogo.UseCases;
using AtendeLogo.UseCases.Common.Excpetions;
using FluentValidation;

namespace AtendeLogo.Application.UnitTests.Registrars;

public class CommandValidationRegistrarTests
{
    [Fact]
    public void CommandValidationRegistrar_ShouldRegisterHandlersCorrectly()
    {
        // Arrange
        Type[] validationsType = [typeof(MockCommandRequest), typeof(MockCommandRequestValidation)];

        var serviceProvider = new ServiceCollection()
            .AddCommandValidationServicesFromTypes(validationsType)
            .BuildServiceProvider();
 
        // Act
        var commandValidation = serviceProvider.GetService<IValidator<MockCommandRequest>>();

        // Assert
        commandValidation.Should().NotBeNull();
        commandValidation.Should().BeOfType<MockCommandRequestValidation>();
     }

    [Fact]
    public void CommandValidationRegistrar_ShouldThrowCommandValidatorNotFound()
    {
        // Arrange
        Type[] validationsType = [typeof(MockCommandRequest)];
        var services = new ServiceCollection();

        // Act
        Action act = () => services.AddCommandValidationServicesFromTypes(validationsType);

        // Assert
        act.Should().Throw<CommandValidatorNotFoundException>();
    }

    [Fact]
    public void CommandValidationRegistrar_ShouldThrowDuplicedCommandValidatorNotFound()
    {
        // Arrange
        Type[] validationsType = [
            typeof(MockCommandRequest), 
            typeof(MockCommandRequestValidation),
            typeof(MockDuplicatedCommandRequestValidation)
        ];

        var services = new ServiceCollection();

        // Act
        Action act = () => services.AddCommandValidationServicesFromTypes(validationsType);

        // Assert
        act.Should().Throw<CommandValidatorAlreadyExistsException>();
    }

    public record MockResponse : IResponse
    {

    }

    public record MockCommandRequest : ICommandRequest<MockResponse>
    {
        public Guid ClientRequestId { get; } = Guid.NewGuid();
    }

    public class MockCommandRequestValidation : AbstractValidator<MockCommandRequest>
    {
    }

    public class MockDuplicatedCommandRequestValidation : AbstractValidator<MockCommandRequest>
    {
    }

}
