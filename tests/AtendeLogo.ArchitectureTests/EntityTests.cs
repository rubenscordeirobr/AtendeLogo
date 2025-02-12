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
        var assembly = typeof(EntityBase).Assembly;

        Types.InAssembly(assembly)
            .That()
            .Inherit(typeof(EntityBase))
            .And()
            .AreNotAbstract()
            .Should()
            .BeSealed()
            .GetResult()
            .IsSuccessful
                .Should()
                .BeTrue();
    }

    [Fact]
    public void Entities_PublicPropertiesShouldHavePrivateOrProtectedSetters()
    {
        var assembly = typeof(EntityBase).Assembly;

        var publicProperties = Types.InAssembly(assembly)
              .That()
              .Inherit(typeof(EntityBase))
              .GetTypes()
              .SelectMany(type => type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
              .Where(prop => prop.GetMethod?.IsPublic == true && prop.SetMethod != null);

        var propertiesWithPublicSetters = publicProperties
            .Where(prop => prop.SetMethod?.IsPublic == true);

        string getBecauseMessage() 
            => $"These properties should have private or protected setters:{Environment.NewLine}" +
               $"{string.Join(Environment.NewLine, propertiesWithPublicSetters.Select(p => p.GetPropertyPath()))}";

        propertiesWithPublicSetters.Should()
            .BeEmpty(getBecauseMessage());
    }
}
