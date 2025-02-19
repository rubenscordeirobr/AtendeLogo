using System.Reflection;
using FluentAssertions;
using NetArchTest.Rules;

namespace AtendeLogo.ArchitectureTests;

public class DependencyReferenceTests
{
    private Assembly _commonAssembly;
    private Assembly _domainAssembly;
    private Assembly _sharedKernelAssembly;
    private Assembly _applicationAssembly;
    private Assembly _useCasesAssembly;
    private Assembly _presentationAssembly;

    private Assembly _infrastructureAssembly;
    private Assembly _identityPersistanceAssembly;
    private Assembly _activityPersistanceAssembly;
    private Assembly _useCasesSharedAssembly;

    public Assembly[] _infrastructuresAssemblies;
    public string[] _infrastructuresAssemblyNames;

    public DependencyReferenceTests()
    {

        _commonAssembly = typeof(Common.Guard).Assembly;
        _domainAssembly = typeof(Domain.Primitives.EntityBase).Assembly;

        _applicationAssembly = typeof(Application.ApplicationServiceConfiguration).Assembly;

        _useCasesAssembly = typeof(UseCases.UseCasesServiceConfiguration).Assembly;
        _presentationAssembly = typeof(Presentation.PresentationServiceConfiguration).Assembly;

        _infrastructureAssembly = typeof(Infrastructure.InfrastructureServiceConfiguration).Assembly;
        _identityPersistanceAssembly = typeof(Persistence.Identity.IdentityPersistenceServiceConfiguration).Assembly;
        _activityPersistanceAssembly = typeof(Persistence.Activity.ActivitiyPersistenceServiceConfiguration).Assembly;

        _sharedKernelAssembly = typeof(Shared.ValueObjects.ValueObjectBase).Assembly;
        _useCasesSharedAssembly = typeof(UseCases.Common.Validations.ValidationMessages).Assembly;

        _infrastructuresAssemblies = [
            _infrastructureAssembly,
            _identityPersistanceAssembly,
            _activityPersistanceAssembly
        ];

        _infrastructuresAssemblyNames = [
            _infrastructureAssembly.GetName().Name!,
            _identityPersistanceAssembly.GetName().Name!,
            _activityPersistanceAssembly.GetName().Name!
        ];

        if (_infrastructuresAssemblyNames.Any(x => x == null))
        {
            throw new Exception("One of the infrastructure assembly names is null");
        }
    }

    [Fact]
    public void CommonAssembly_ShouldNotHaveDependencies()
    {
        //Arrange
        string[] dependencies = [
            _domainAssembly.GetName().Name!,
            _applicationAssembly.GetName().Name!,
            _useCasesSharedAssembly.GetName().Name!,
            _useCasesAssembly.GetName().Name!,
            _presentationAssembly.GetName().Name!,
             .._infrastructuresAssemblyNames];

        //Act
        var result = Types.InAssembly(_commonAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(dependencies)
            .GetResult();

        //Assert
        result.IsSuccessful
                .Should()
                .BeTrue($"The CommonAssembly should not have dependencies on Domain, " +
                        $"Application, UseCases, UseCasesShared, Presentation and Infrastructure layers.\r\n" +
                        $"FailingTypeNames {result.GetFailingTypeNames()}");
    }

    [Fact]
    public void UseSharedKernelAssembly_ShouldNotHaveDependencies()
    {
        //Arrange
        string[] depedencies = [
                _domainAssembly.GetName().Name!,
                _applicationAssembly.GetName().Name!,
                _useCasesSharedAssembly.GetName().Name!,
                _presentationAssembly.GetName().Name!,
                .._infrastructuresAssemblyNames];

        //Act
        var result = Types.InAssembly(_sharedKernelAssembly)
               .ShouldNot()
               .HaveDependencyOnAny(depedencies)
               .GetResult();

        //Assert
        result.IsSuccessful
            .Should()
            .BeTrue($"The SharedKernelAssembly should not have dependencies on Domain, " +
                    $"Application, UseCasesShared, Presentation and Infrastructure layers.\r\n" +
                    $"FailingTypeNames {result.GetFailingTypeNames()}");
    }

    [Fact]
    public void ApplicationAssembly_ShouldNotHaveInfrastructureDependencies()
    {
        //Arrange
        string[] dependencies = [
         _useCasesAssembly.GetName().Name!,
             .._infrastructuresAssemblyNames];

        //Act
        var result = Types.InAssembly(_applicationAssembly)
             .ShouldNot()
             .HaveDependencyOnAny(dependencies)
             .GetResult();

        //Assert
        result.IsSuccessful
            .Should()
            .BeTrue($"The ApplicationAssembly should not have dependencies on UseCases and Infrastructure layers.\r\n" +
                    $"FailingTypeNames {result.GetFailingTypeNames()}");
    }

    [Fact]
    public void UserCasesAssembly_ShouldNotHaveDependencies()
    {
        //Arrange
        string[] dependencies = [.. _infrastructuresAssemblyNames];

        //Act
        var result = Types.InAssembly(_applicationAssembly)
              .ShouldNot()
              .HaveDependencyOnAny(dependencies)
              .GetResult();

        //Assert
        result.IsSuccessful
            .Should()
            .BeTrue($"The UseCasesAssembly should not have dependencies on Infrastructure layers.\r\n" +
                    $"FailingTypeNames {result.GetFailingTypeNames()}");
    }

    [Fact]
    public void UseCasesSharedAssembly_ShouldNotHaveDomainAndInfrastructureDependencies()
    {
        //Arrange
        string[] dependencies = [
           _domainAssembly.GetName().Name!,
           _applicationAssembly.GetName().Name!,
           _presentationAssembly.GetName().Name!,
             .._infrastructuresAssemblyNames];

        //Act
        var result = Types.InAssembly(_useCasesSharedAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(dependencies)
            .GetResult();

        //Assert
        result.IsSuccessful
                .Should()
                .BeTrue($"The UseCasesSharedAssembly should not have dependencies on Domain, " +
                        $"Application, UseCases and Infrastructure layers.\r\n" +
                        $"FailingTypeNames {result.GetFailingTypeNames()}");
    }

    [Fact]
    public void PresentationAssembly_ShouldNotHaveDomainDependencies()
    {
        //Arrange
        var domainAssemblyName = _domainAssembly.GetName().Name!;

        //Act
        var result = Types.InAssembly(_presentationAssembly)
            .ShouldNot()
            .HaveDependencyOn(domainAssemblyName)
            .GetResult();

        //Assert
        result.IsSuccessful
               .Should()
               .BeTrue(because: "Presentation layer should not have dependencies on Domain layer." +
                                $" FailingTypeNames { result.GetFailingTypeNames()}");

    }
}
