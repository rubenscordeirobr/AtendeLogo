namespace AtendeLogo.Common.UnitTests.Utils;

public class JsonSchemaUtilsTests
{
    public static IEnumerable<object[]> GetJsonSchemaTypeTestData()
    {
        yield return new object[] { typeof(string), "string" };
        yield return new object[] { typeof(bool), "boolean" };

        // Integer types
        yield return new object[] { typeof(int), "integer" };
        yield return new object[] { typeof(long), "integer" };
        yield return new object[] { typeof(short), "integer" };
        yield return new object[] { typeof(byte), "integer" };
        yield return new object[] { typeof(uint), "integer" };
        yield return new object[] { typeof(ulong), "integer" };
        yield return new object[] { typeof(ushort), "integer" };

        // Number types
        yield return new object[] { typeof(float), "number" };
        yield return new object[] { typeof(double), "number" };
        yield return new object[] { typeof(decimal), "number" };

        // Enum type
        yield return new object[] { typeof(TestEnum), "number" };

        // Enumerable type (excluding string)
        yield return new object[] { typeof(int[]), "array" };

        // Nullable types
        yield return new object[] { typeof(int?), "integer" };
        yield return new object[] { typeof(bool?), "boolean" };

        // Default object type
        yield return new object[] { typeof(JsonSchemaUtilsTests), "object" };
    }

    [Theory]
    [MemberData(nameof(GetJsonSchemaTypeTestData))]
    public void GetJsonSchemaType_ShouldReturnExpectedType(Type inputType, string expectedSchemaType)
    {
        // Act
        var schemaType = JsonSchemaUtils.GetJsonSchemaType(inputType);

        // Assert
        schemaType.Should().Be(expectedSchemaType);
    }

    private enum TestEnum
    {
        First,
        Second,
        Third
    }
}

