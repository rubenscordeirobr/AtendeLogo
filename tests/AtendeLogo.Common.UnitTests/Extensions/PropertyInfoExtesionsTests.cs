namespace AtendeLogo.Common.UnitTests.Extensions;
public class PropertyInfoExtesionsTests
{
    private class NestedPropertyTestClass
    {
        public string? TestProperty { get; set; }
    }

    [Theory]
    [InlineData(typeof(NestedPropertyTestClass), nameof(NestedPropertyTestClass.TestProperty), "AtendeLogo.Common.UnitTests.Extensions.PropertyInfoExtesionsTests+NestedTestClass::TestProperty")]
    [InlineData(typeof(TestPropertyPathClass), nameof(TestPropertyPathClass.TestProperty), "AtendeLogo.Common.UnitTests.Extensions.TestClass::TestProperty")]
    public void GetPropertyPath_ShouldReturnCorrectPath(Type type, string propertyName, string result)
    {
        // Arrange
        var propertyInfo = type.GetProperty(propertyName);

        // Act
        var propertyPath = propertyInfo?.GetPropertyPath();

        // Assert
        propertyPath.Should().Be(result);
    }
}

internal class TestPropertyPathClass
{
    public string? TestProperty { get; set; }
}
