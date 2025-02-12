using System.Reflection;
using FluentAssertions;
using NetArchTest.Rules;

namespace AtendeLogo.ArchitectureTests;

public class DependencyReferenceTests
{
    private Assembly _commomAssembly;
    private Assembly _domainAssembly;
    private Assembly _applicationAssembly;
    private Assembly _useCasesAssembly;
    private Assembly _infrastructureAssembly;
    private Assembly _persistanceAssembly;
    private Assembly _persistanceActivitiesAssembly;

    private Assembly _useCasesSharedAssembly;
    private Assembly _sharedKernelAssembly;

    public DependencyReferenceTests()
    {

        _commomAssembly = typeof(Common.Guard).Assembly;
        _domainAssembly = typeof(Domain.Primitives.EntityBase).Assembly;

        _applicationAssembly = typeof(Application.ApplicationConfigurationsExtensions).Assembly;

        _useCasesAssembly = typeof(UseCases.UseCasesConfigurationExtensions).Assembly;
        _infrastructureAssembly = typeof(Infrastructure.InfrastructureConfigurationExtensions).Assembly;
        _persistanceAssembly = typeof(Persistence.PersistenceConfigurationExtensions).Assembly;
        _persistanceActivitiesAssembly = typeof(Persistence.Activities.ActivitiesPersistenceConfigurationExtensions).Assembly;

        _sharedKernelAssembly = typeof(Shared.ValueObjects.ValueObjectBase).Assembly;
        _useCasesSharedAssembly = typeof(UseCases.Resources.ValidationMessages).Assembly;

    }

    [Fact]
    public void CommomAssembly_ShouldNotHaveDependencies()
    {
        var result = Types.InAssembly(_commomAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(
                _useCasesSharedAssembly.GetName().Name,
                _domainAssembly.GetName().Name,
                _applicationAssembly.GetName().Name,
                _infrastructureAssembly.GetName().Name,
                _persistanceAssembly.GetName().Name,
                _persistanceActivitiesAssembly.GetName().Name)
            .GetResult().IsSuccessful
            .Should()
            .BeTrue();
    }

    [Fact]
    public void UseSharedKernelAssembly_ShouldNotHaveDependencies()
    {
        Types.InAssembly(_sharedKernelAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(
                _useCasesSharedAssembly.GetName().Name,
                _domainAssembly.GetName().Name,
                _applicationAssembly.GetName().Name,
                _infrastructureAssembly.GetName().Name,
                _persistanceAssembly.GetName().Name,
                _persistanceActivitiesAssembly.GetName().Name)
            .GetResult()
            .IsSuccessful
            .Should()
            .BeTrue();
    }

    [Fact]
    public void UseCasesSharedAssembly_ShouldNotHaveDependencies()
    {
        Types.InAssembly(_useCasesSharedAssembly)
           .ShouldNot()
           .HaveDependencyOnAny(
               _domainAssembly.GetName().Name,
               _applicationAssembly.GetName().Name,
               _infrastructureAssembly.GetName().Name,
               _persistanceAssembly.GetName().Name,
               _persistanceActivitiesAssembly.GetName().Name)
           .GetResult()
           .IsSuccessful
                .Should()
                .BeTrue();
    }

    [Fact]
    public void ApplicationdAssembly_ShouldNotHaveDependencies()
    {
        Types.InAssembly(_applicationAssembly)
             .ShouldNot()
             .HaveDependencyOnAny(
                 _infrastructureAssembly.GetName().Name,
                 _useCasesAssembly.GetName().Name,
                 _persistanceAssembly.GetName().Name,
                 _persistanceActivitiesAssembly.GetName().Name)
             .GetResult()
             .IsSuccessful
                .Should()
                .BeTrue();
    }

    [Fact]
    public void UserCasesAssembly_ShouldNotHaveDependencies()
    {
        Types.InAssembly(_applicationAssembly)
             .ShouldNot()
             .HaveDependencyOnAny(
                 _infrastructureAssembly.GetName().Name,
                 _persistanceAssembly.GetName().Name,
                 _persistanceActivitiesAssembly.GetName().Name)
             .GetResult()
             .IsSuccessful
                .Should()
                .BeTrue();
     }
}
