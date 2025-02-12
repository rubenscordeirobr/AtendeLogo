using System.ComponentModel.DataAnnotations.Schema;

namespace AtendeLogo.Common.UnitTests.Extensions;

public class TypeExtensionsTests
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

    public static IEnumerable<object[]> IsImplementsGenericInterfaceDefinitionData
        => new List<object[]>
        {
            new object[] { typeof(List<string>), typeof(IEnumerable<>), true },
            new object[] { typeof(List<string>), typeof(IList<>), true },
            new object[] { typeof(List<string>), typeof(ICollection<>), true },
            new object[] { typeof(string), typeof(IEnumerable<>), true },
            new object[] { typeof(DerivedClass), typeof(IEnumerable<>), false },
        };

    [Theory]
    [MemberData(nameof(IsImplementsGenericInterfaceDefinitionData))]
    public void IsImplementsGenericInterfaceDefinition_ShouldReturnExpectedResult(
        Type givenType,
        Type genericType,
        bool expectedResult)
    {
        givenType.ImplementsGenericInterfaceDefinition(genericType).Should().Be(expectedResult);
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

    public static IEnumerable<object[]> GetAssignableTypesTestData
        => new List<object[]>
        {
            new object[] { typeof(DerivedClass), new[] { typeof(DerivedClass), typeof(BaseClass) } },
            new object[] { typeof(BaseClass), new[] { typeof(BaseClass) } },
            new object[] { typeof(List<string>), new[] { typeof(List<string>), typeof(IEnumerable<string>), typeof(ICollection<string>), typeof(IList<string>) } },
        };

    [Theory]
    [MemberData(nameof(GetAssignableTypesTestData))]
    public void GetAssignableTypes_ShouldReturnExpectedResult(Type type, Type[] expectedTypes)
    {
        var result = type.GetAssignableTypes().ToArray();
        result.Should().Contain(expectedTypes);
    }

    public static IEnumerable<object[]> GetQualifiedTypeNameTestData
        => new List<object[]>
        {
            new object[] { typeof(string), "System.String" },
            new object[] { typeof(List<string>), "List<System.String>" },
            new object[] { typeof(Dictionary<int, string>), "Dictionary<System.Int32, System.String>" },
        };

    [Theory]
    [MemberData(nameof(GetQualifiedTypeNameTestData))]
    public void GetQualifiedName_ShouldReturnExpectedResult(Type type, string expectedName)
    {
        var result = type.GetQualifiedName();
        result.Should().Be(expectedName);
    }

    public static IEnumerable<object[]> GetPropertiesFromInterfaceTestData
        => new List<object[]>
        {
            new object[] { typeof(TestClassWithInterface), typeof(ITestInterface), new[] { "Property1", "Property2" } },
        };

    [Theory]
    [MemberData(nameof(GetPropertiesFromInterfaceTestData))]
    public void GetPropertiesFromInterface_ShouldReturnExpectedResult(Type type, Type interfaceType, string[] expectedProperties)
    {
        var result = type.GetPropertiesFromInterface(interfaceType).Keys.ToArray();
        result.Should().BeEquivalentTo(expectedProperties);
    }

    private interface ITestInterface
    {
        string? Property1 { get; set; }
        int Property2 { get; set; }
    }

    private class TestClassWithInterface : ITestInterface
    {
        public string? Property1 { get; set; }
        public int Property2 { get; set; }
        public string? Property3 { get; set; }
    }

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
