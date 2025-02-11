namespace AtendeLogo.Common.UnitTests.Extensions;

public class DictionaryExtensionsTest
{
    [Fact]
    public void GetOrAdd_KeyExists_ReturnsExistingValue()
    {
        // Arrange
        var dictionary = new Dictionary<string, int> { { "key1", 1 } };

        // Act
        var result = dictionary.GetOrAdd("key1", () => 2);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void GetOrAdd_KeyDoesNotExist_AddsAndReturnsNewValue()
    {
        // Arrange
        var dictionary = new Dictionary<string, int>();

        // Act
        var result = dictionary.GetOrAdd("key1", () => 2);

        // Assert
        Assert.Equal(2, result);
        Assert.Equal(2, dictionary["key1"]);
    }

    [Fact]
    public void GetOrAdd_ValueFactoryIsCalledOnce()
    {
        // Arrange
        var dictionary = new Dictionary<string, int>();
        var callCount = 0;

        // Act
        var result = dictionary.GetOrAdd("key1", () =>
        {
            callCount++;
            return 2;
        });

        // Assert
        Assert.Equal(1, callCount);
        Assert.Equal(2, result);
    }
}
