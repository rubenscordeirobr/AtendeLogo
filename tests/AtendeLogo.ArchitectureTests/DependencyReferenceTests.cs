using System.Reflection;
using FluentAssertions;
using NetArchTest.Rules;

namespace AtendeLogo.ArchitectureTests;

public class DependencyReferenceTests
{
    private Assembly _commonAssembly;
    private Assembly _domainAssembly;
    private Assembly _applicationAssembly;
    private Assembly _useCasesAssembly;
    private Assembly _infrastructureAssembly;
    private Assembly _identityPersistanceAssembly;
    private Assembly _activityPersistanceAssembly;

    private Assembly _useCasesSharedAssembly;
    private Assembly _sharedKernelAssembly;

    public DependencyReferenceTests()
    {

        _commonAssembly = typeof(Common.Guard).Assembly;
        _domainAssembly = typeof(Domain.Primitives.EntityBase).Assembly;

        _applicationAssembly = typeof(Application.ApplicationServiceConfiguration).Assembly;

        _useCasesAssembly = typeof(UseCases.UseCasesServiceConfiguration).Assembly;
        _infrastructureAssembly = typeof(Infrastructure.InfrastructureServiceConfiguration).Assembly;
        _identityPersistanceAssembly = typeof(Persistence.Identity.IdentityPersistenceServiceConfiguration).Assembly;
        _activityPersistanceAssembly = typeof(Persistence.Activity.ActivitiyPersistenceServiceConfiguration).Assembly;

        _sharedKernelAssembly = typeof(Shared.ValueObjects.ValueObjectBase).Assembly;
        _useCasesSharedAssembly = typeof(UseCases.Resources.ValidationMessages).Assembly;
    }

    [Fact]
    public void CommonAssembly_ShouldNotHaveDependencies()
    {
        var result = Types.InAssembly(_commonAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(
                _useCasesSharedAssembly.GetName().Name,
                _domainAssembly.GetName().Name,
                _applicationAssembly.GetName().Name,
                _infrastructureAssembly.GetName().Name,
                _identityPersistanceAssembly.GetName().Name,
                _activityPersistanceAssembly.GetName().Name)
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
                _identityPersistanceAssembly.GetName().Name,
                _activityPersistanceAssembly.GetName().Name)
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
               _identityPersistanceAssembly.GetName().Name,
               _activityPersistanceAssembly.GetName().Name)
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
                 _identityPersistanceAssembly.GetName().Name,
                 _activityPersistanceAssembly.GetName().Name)
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
                 _identityPersistanceAssembly.GetName().Name,
                 _activityPersistanceAssembly.GetName().Name)
             .GetResult()
             .IsSuccessful
                .Should()
                .BeTrue();
     }
}
