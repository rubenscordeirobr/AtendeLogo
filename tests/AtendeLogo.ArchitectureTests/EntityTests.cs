using System.Reflection;
using AtendeLogo.Common.Extensions;
using AtendeLogo.Domain.Primitives;
using FluentAssertions;
using NetArchTest.Rules;

namespace AtendeLogo.ArchitectureTests;

public class EntityTests
{
    [Fact]
    public void Entities_ShouldBeSealed()
    {
        // Arrange
        var assembly = typeof(EntityBase).Assembly;

        //
        var result = Types.InAssembly(assembly)
             .That()
             .Inherit(typeof(EntityBase))
             .And()
             .AreNotAbstract()
             .Should()
             .BeSealed()
             .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue(
            because: "Entities should be sealed or abstract to prevent modification." +
                     $"Entities fail :{result.GetFailingTypeNames()}");
    }

    [Fact]
    public void Entities_PublicPropertiesShouldHavePrivateOrProtectedSetters()
    {
        // Arrange
        var assembly = typeof(EntityBase).Assembly;

        //Act
        var publicPropertiesWithPublicSetters = Types.InAssembly(assembly)
              .That()
              .Inherit(typeof(EntityBase))
              .GetTypes()
              .SelectMany(type => type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
              .Where(prop => prop.GetMethod?.IsPublic == true && prop.SetMethod != null)
              .Where(prop => prop.SetMethod?.IsPublic == true);
         
        // Assert
        publicPropertiesWithPublicSetters.Should()
            .BeEmpty(getBecauseMessage());

        string getBecauseMessage()
            => $"These properties should have private or protected setters:{Environment.NewLine}" +
               $"{string.Join(Environment.NewLine, publicPropertiesWithPublicSetters.Select(p => p.GetPropertyPath()))}";
    }
}
