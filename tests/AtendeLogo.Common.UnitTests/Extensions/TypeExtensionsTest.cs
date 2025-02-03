using System.ComponentModel.DataAnnotations.Schema;

namespace AtendeLogo.Common.UnitTests.Extensions;

public class TypeExtensionsTest
{
    public static IEnumerable<object[]> IsSubclassOfTestData
        => new List<object[]>
        {
            new object[] { typeof(DerivedClass), typeof(BaseClass), true },
            new object[] { typeof(BaseClass), typeof(DerivedClass), false },
            new object[] { typeof(object), typeof(BaseClass), false },
            new object[] { typeof(DerivedClass), typeof(object), true },
        };

    [Theory]
    [MemberData(nameof(IsSubclassOfTestData))]
    public void IsSubclassOf_ShouldReturnExpectedResult(
        Type type,
        Type baseType,
        bool expectedResult)
    {
        type.IsSubclassOf(baseType).Should().Be(expectedResult);
    }

    public static IEnumerable<object[]> IsSubclassOfOrEqualsTestData 
        => new List<object[]>
        {
            new object[] { typeof(DerivedClass), typeof(BaseClass), true },
            new object[] { typeof(BaseClass), typeof(DerivedClass), false },
            new object[] { typeof(BaseClass), typeof(BaseClass), true },
            new object[] { typeof(object), typeof(BaseClass), false },
            new object[] { typeof(DerivedClass), typeof(object), true },
        };

    [Theory]
    [MemberData(nameof(IsSubclassOfOrEqualsTestData))]
    public void IsSubclassOfOrEquals_ShouldReturnExpectedResult(
        Type type,
        Type otherType,
        bool expectedResult)
    {
        type.IsSubclassOfOrEquals(otherType).Should().Be(expectedResult);
    }

    public static IEnumerable<object[]> IsAssignableToGenericTypeTestData
        => new List<object[]>
        {
            new object[] { typeof(List<string>), typeof(IEnumerable<>), true },
            new object[] { typeof(List<string>), typeof(IList<>), true },
            new object[] { typeof(List<string>), typeof(ICollection<>), true },
            new object[] { typeof(string), typeof(IEnumerable<>), true },
            new object[] { typeof(DerivedClass), typeof(IEnumerable<>), false },
        };

    [Theory]
    [MemberData(nameof(IsAssignableToGenericTypeTestData))]
    public void IsAssignableToGenericType_ShouldReturnExpectedResult(
        Type givenType,
        Type genericType,
        bool expectedResult)
    {
        givenType.IsAssignableToGenericType(genericType).Should().Be(expectedResult);
    }

    public static IEnumerable<object[]> GetDeclaredPropertiesTestData
        => new List<object[]>
        {
            new object[] { typeof(TestClass), true, new[] { "Property1", "Property2" } },
            new object[] { typeof(TestClass), false, new[] { "Property1", "Property2", "NotMappedProperty" } },
        };

    [Theory]
    [MemberData(nameof(GetDeclaredPropertiesTestData))]
    public void GetDeclaredProperties_ShouldReturnExpectedResult(
        Type type,
        bool isIgnoreNotMappedAtribute,
        string[] expectedProperties)
    {
        var properties = type.GetDeclaredProperties(isIgnoreNotMappedAtribute)
            .Select(p => p.Name)
            .ToArray();

        properties.Should().BeEquivalentTo(expectedProperties);
    }

    public static IEnumerable<object[]> GetDeclaredPropertiesOfTypeTestData
        => new List<object[]>
        {
            new object[] { typeof(TestClass), typeof(string), true, new[] { "Property1" } },
            new object[] { typeof(TestClass), typeof(int), true, new[] { "Property2" } },
            new object[] { typeof(TestClass), typeof(string), false, new[] { "Property1", "NotMappedProperty" } },
        };

    [Theory]
    [MemberData(nameof(GetDeclaredPropertiesOfTypeTestData))]
    public void GetDeclaredPropertiesOfType_ShouldReturnExpectedResult(
        Type type,
        Type propertyType,
        bool isIgnoreNotMappedAtribute,
        string[] expectedProperties)
    {
        var properties = type.GetDeclaredPropertiesOfType(propertyType, isIgnoreNotMappedAtribute)
            .Select(p => p.Name)
            .ToArray();

        properties.Should().BeEquivalentTo(expectedProperties);
    }

    private class BaseClass { }
    private class DerivedClass : BaseClass { }

    private class TestClass
    {
        public string? Property1 { get; set; }
        public int Property2 { get; set; }

        [NotMapped]
        public string? NotMappedProperty { get; set; }
    }
}
