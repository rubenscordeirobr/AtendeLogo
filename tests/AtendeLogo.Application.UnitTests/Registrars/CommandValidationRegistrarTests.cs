using AtendeLogo.Shared.Contracts;
using AtendeLogo.UseCases;
using AtendeLogo.UseCases.Excpetions;
using FluentValidation;
using Microsoft.Extensions.Hosting;

namespace AtendeLogo.Application.UnitTests.Registrars;

public class CommandValidationRegistrarTests
{
    [Fact]
    public void CommandValidationRegistrar_ShouldRegisterHandlersCorrectly()
    {
        // Arrange
        Type[] validationsType = [typeof(MockCommandRequest), typeof(MockCommandRequestValidation)];

        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddCommandValidationServicesFromTypes(validationsType);
        
        var app = builder.Build();
        var serviceProvider = app.Services;

        // Act
        var commandValidation = serviceProvider.GetService<IValidator<MockCommandRequest>>();

        // Assert
        commandValidation.Should().NotBeNull();
        commandValidation.Should().BeOfType<MockCommandRequestValidation>();

        app.Dispose();
    }

    [Fact]
    public void CommandValidationRegistrar_ShouldThrowCommandValidatorNotFound()
    {
        // Arrange
        Type[] validationsType = [typeof(MockCommandRequest)];
        var builder = Host.CreateApplicationBuilder();

        // Act
        Action act = () => builder.Services.AddCommandValidationServicesFromTypes(validationsType);

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

        var builder = Host.CreateApplicationBuilder();

        // Act
        Action act = () => builder.Services.AddCommandValidationServicesFromTypes(validationsType);

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
