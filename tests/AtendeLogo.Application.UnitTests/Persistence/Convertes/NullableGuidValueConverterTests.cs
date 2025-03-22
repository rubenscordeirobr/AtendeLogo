namespace AtendeLogo.Persistence.Tests.Converters;

public class NullableGuidValueConverterTests
{
    [Fact]
    public void ConvertToProvider_With_ValidGuid_Should_Return_SameGuid()
    { 
        // Arrange
        var validGuid = Guid.NewGuid();
        var converter = NullableGuidValueConverter.Instance;
        var convertToProvider = converter.ConvertToProviderExpression.Compile();

        // Act
        Guid? result = convertToProvider(validGuid);

        // Assert
        result.Should().Be(validGuid);
    }

    [Fact]
    public void ConvertToProvider_With_GuidEmpty_Should_Return_Null()
    {
        // Arrange
        var converter = NullableGuidValueConverter.Instance;
        var convertToProvider = converter.ConvertToProviderExpression.Compile();

        // Act
        Guid? result = convertToProvider(Guid.Empty);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void ConvertToProvider_With_Null_Should_Return_Null()
    {
        // Arrange
        var converter = NullableGuidValueConverter.Instance;
        var convertToProvider = converter.ConvertToProviderExpression.Compile();

        // Act
        Guid? result = convertToProvider(null);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void ConvertFromProvider_With_ValidGuid_Should_Return_SameGuid()
    {
        // Arrange
        var validGuid = Guid.NewGuid();
        var converter = NullableGuidValueConverter.Instance;
        var convertFromProvider = converter.ConvertFromProviderExpression.Compile();

        // Act
        Guid? result = convertFromProvider(validGuid);

        // Assert
        result.Should().Be(validGuid);
    }

    [Fact]
    public void ConvertFromProvider_With_GuidEmpty_Should_Return_Null()
    {
        // Arrange
        var converter = NullableGuidValueConverter.Instance;
        var convertFromProvider = converter.ConvertFromProviderExpression.Compile();

        // Act
        Guid? result = convertFromProvider(Guid.Empty);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void ConvertFromProvider_With_Null_Should_Return_Null()
    {
        // Arrange
        var converter = NullableGuidValueConverter.Instance;
        var convertFromProvider = converter.ConvertFromProviderExpression.Compile();

        // Act
        Guid? result = convertFromProvider(null);

        // Assert
        result.Should().BeNull();
    }
}
