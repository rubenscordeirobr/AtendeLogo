using System.Reflection;
using AtendeLogo.ArchitectureTests.TestSupport;
using AtendeLogo.Common.Extensions;
using AtendeLogo.Domain.Primitives;
using FluentAssertions;
using Xunit.Abstractions;

namespace AtendeLogo.ArchitectureTests;

public class EntityValidationTests : IClassFixture<ApplicationAssemblyContext>
{
    private readonly ITestOutputHelper _output;
    private readonly IReadOnlyDictionary<Type, Type> _entityTypeToConfigurationTypeMap;

    public EntityValidationTests(
        ApplicationAssemblyContext assemblyContext,
        ITestOutputHelper output)
    {
        _entityTypeToConfigurationTypeMap = assemblyContext.EntityTypeToConfigurationTypeMap;
        _output = output;
    }

    public static IEnumerable<object[]> EntityTypes
    {
        get
        {
            return new ApplicationAssemblyContext()
                 .EntityTypes
                 .Select(type => new object[] { type });
        }
    }

    [Theory]
    [MemberData(nameof(EntityTypes))]
    public void EntityType_ShouldBe_AbstractOrSealed(Type entityType)
    {
        //Act
        var result = entityType.IsClass && (entityType.IsAbstract || entityType.IsSealed);

        // Assert
        result.Should()
            .BeTrue($"The entity {entityType.Name} is not abstract or sealed." +
                    "Entities should be sealed or abstract to prevent modification.");

        _output.WriteLine($"Entity {entityType.Name} is abstract or sealed");
    }

    [Theory]
    [MemberData(nameof(EntityTypes))]
    public void EntityType_ShouldHave_PublicProperties_WithPrivateOrProtectedSetters(
        Type entityType)
    {
        // Arrange
        var assembly = typeof(EntityBase).Assembly;

        //Act
        var publicPropertiesWithPublicSetters = entityType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
              .Where(prop => prop.GetMethod?.IsPublic == true && prop.SetMethod != null)
              .Where(prop => prop.SetMethod?.IsPublic == true);

        // Assert
        publicPropertiesWithPublicSetters.Should()
            .BeEmpty(getBecauseMessage());

        string getBecauseMessage()
            => $"These properties should have private or protected setters:{Environment.NewLine}" +
               $"{string.Join(Environment.NewLine, publicPropertiesWithPublicSetters.Select(p => p.GetPropertyPath()))}";

        _output.WriteLine($"Entity {entityType.Name} has no public properties with public setters");
    }

    [Theory]
    [MemberData(nameof(EntityTypes))]
    public void EntityType_ShouldHave_EFCoreConfiguration(Type entityType)
    {
        // Arrange
        var entityConfigurationType = _entityTypeToConfigurationTypeMap.GetValueOrDefault(entityType);

        // Assert
        entityConfigurationType.Should()
            .NotBeNull($"The entity {entityType.Name} does not have a configuration");

        _output.WriteLine($"Entity {entityType.Name} has configuration {entityConfigurationType!.Name}");
    }
}
