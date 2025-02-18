namespace AtendeLogo.Common.UnitTests.Extensions;

public class GuidExtensionsTests
{
    [Fact]
    public void IsZeroPrefixedGuid_ShouldReturnTrue_WhenGuidIsZeroPrefixed()
    {
        // Arrange
        var guid = GuidExtensions.NewGuidZeroPrefixed();

        // Act
        var result = guid.IsZeroPrefixedGuid();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsZeroPrefixedGuid_ShouldReturnFalse_WhenGuidIsNotZeroPrefixed()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var result = guid.IsZeroPrefixedGuid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void NewGuidZeroPrefixed_ShouldReturnGuidWithZeroPrefix()
    {
        // Act
        var guid = GuidExtensions.NewGuidZeroPrefixed();

        // Assert
        var bytes = guid.ToByteArray();
        bytes[0].Should().Be(0x0);
        bytes[1].Should().Be(0x0);
        bytes[2].Should().Be(0x0);
        bytes[3].Should().Be(0x0);
        bytes[4].Should().Be(0x0);
    }
}

