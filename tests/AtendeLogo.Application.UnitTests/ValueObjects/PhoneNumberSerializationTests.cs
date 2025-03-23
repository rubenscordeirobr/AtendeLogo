namespace AtendeLogo.Application.UnitTests.ValueObjects;

public class PhoneNumberSerializationTests
{
    [Fact]
    public void Serialize_ShouldProduceValidJson_WithComputedNumber()
    {
        // Arrange
        var phoneNumberInput = "5511987651234";
        var expectedValue = $"\\\\u002B{phoneNumberInput}";
        var phoneNumber = new PhoneNumber('+' + phoneNumberInput);

        // Act
        var json = JsonUtils.Serialize(phoneNumber);

        // Assert
        json.Should()
           .MatchRegex(@$"""number""\s*:\s*""{expectedValue}""");
    }

    [Fact]
    public void Deserialize_ShouldCreatePhoneNumberCorrectly()
    {
        // Arrange
        var phoneNumberInput = "+5511987651234";
        // The JsonConstructor expects a property named "number" (case-insensitive by default).
        var json = $"{{\"number\":\"{phoneNumberInput}\"}}";

        // Act
        var phoneNumber = JsonUtils.Deserialize<PhoneNumber>(json);

        // Assert
        phoneNumber.Should().NotBeNull();
        phoneNumber!.Number.Should().Be(phoneNumberInput);
    }
}
