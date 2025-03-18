using System.Text.Json;
using AtendeLogo.Common.Helpers;

namespace AtendeLogo.Common.UnitTests.Helpers;

public static class JsonSerializerOptionsHelperTests
{
    [Fact]
    public static void EnableIndentationInDevelopment_ShouldEnableIndentation_WhenEnvironmentIsDevelopment_AndWriteIndentedIsFalse()
    {
        // Arrange
        var originalEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        try
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            var options = new JsonSerializerOptions
            {
                WriteIndented = false
            };

            // Act
            JsonSerializerOptionsHelper.EnableIndentationInDevelopment(options);

            // Assert
            options.WriteIndented.Should().BeTrue();
        }
        finally
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", originalEnvironment);
        }
    }

    [Fact]
    public static void EnableIndentationInDevelopment_ShouldNotChangeIndentation_WhenWriteIndentedIsAlreadyTrue()
    {
        // Arrange
        var originalEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        try
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            // Act
            JsonSerializerOptionsHelper.EnableIndentationInDevelopment(options);

            // Assert
            options.WriteIndented.Should().BeTrue();
        }
        finally
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", originalEnvironment);
        }
    }

    [Fact]
    public static void EnableIndentationInDevelopment_ShouldNotEnableIndentation_WhenEnvironmentIsNotDevelopment()
    {
        // Arrange
        var originalEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        try
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production");
            var options = new JsonSerializerOptions
            {
                WriteIndented = false
            };

            // Act
            JsonSerializerOptionsHelper.EnableIndentationInDevelopment(options);

            // Assert
            options.WriteIndented.Should().BeFalse();
        }
        finally
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", originalEnvironment);
        }
    }

    [Fact]
    public static void EnableIndentationInDevelopment_ShouldThrowArgumentNullException_WhenOptionsIsNull()
    {
        // Arrange
        Action act = () => JsonSerializerOptionsHelper.EnableIndentationInDevelopment(null!);

        // Act & Assert
        act.Should().Throw<ArgumentNullException>();
    }
}
