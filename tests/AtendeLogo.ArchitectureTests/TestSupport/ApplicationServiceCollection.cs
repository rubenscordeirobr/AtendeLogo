using AtendeLogo.Application;
using AtendeLogo.Application.Contracts.Handlers;
using AtendeLogo.Application.Contracts.Persistence;
using AtendeLogo.Application.Contracts.Services;
using AtendeLogo.Application.UnitTests.Mocks;
using AtendeLogo.ArchitectureTests.Extensions;
using AtendeLogo.Infrastructure;
using AtendeLogo.Persistence.Activity;
using AtendeLogo.Persistence.Identity;
using AtendeLogo.Presentation;
using AtendeLogo.UseCases;
using AtendeLogo.UseCases.Common.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using StackExchange.Redis;

namespace AtendeLogo.ArchitectureTests.TestSupport ;

public class ApplicationServiceCollection : ServiceCollection
{
    public IReadOnlyList<ServiceDescriptor> ApplicationServices { get; }
    public IReadOnlyList<Type> ImplementedServiceTypes { get; }

    public ApplicationServiceCollection()
    {
        var configurations = new Dictionary<string, string> {
            { "ConnectionStrings:IdentityPostgresql", "Host=postgress;Port=5432;Database=atende_logo;Username=atende_logo_user;Password=Temp@Teste123%$;Include Error Detail=true" },
            { "ConnectionStrings:ActivityMongoDb", "mongodb://atende_logo_user:Temp%40Teste123%25%24@localhost:27017/?authSource=admin&readPreference=primary&ssl=false&directConnection=true" },
            { "ConnectionStrings:CacheRedis", "redis:6379,password=Temp@Teste123%$" }
        };

        var fakeConfiguration = new ConfigurationBuilder()
            .AddInMemoryCollection(configurations!)
            .Build();

        this.AddApplicationServices()
            .AddUserCasesServices()
            .AddUserCasesSharedServices()
            .AddPresentationServices()
            .AddInfrastructureServices(fakeConfiguration)
            .AddIdentityPersistenceServices(fakeConfiguration)
            .AddActivityPersistenceServices(fakeConfiguration);

        AddMockDependency(fakeConfiguration);
         
        ApplicationServices = RetrieveApplicationServices();
        ImplementedServiceTypes = GetImplementedServiceTypes();
    }

    private void AddMockDependency(IConfigurationRoot fakeConfiguration)
    {
        this.AddScoped<IHttpContextAccessor, HttpContextAccessorMock>();
        this.AddTransient(typeof(ILogger<>), typeof(LoggerServiceMock<>));
        this.AddSingleton<IConfiguration>(fakeConfiguration);

        this.RemoveAll<IConnectionMultiplexer>();
        this.AddSingleton(new Mock<IConnectionMultiplexer>().Object);
        this.AddSingleton(new Mock<IHostEnvironment>().Object);
    }

    private IReadOnlyList<ServiceDescriptor> RetrieveApplicationServices()
    {
        Type[] targetTypes = [
            typeof(IRepositoryBase<>),
            typeof(IApplicationHandler),
            typeof(IValidator),
            typeof(IValidationService),
            typeof(IApplicationService)
       ];

        return this
            .Where(descriptor => descriptor.IsAssignableTo(targetTypes))
            .ToList();
    }

    private List<Type> GetImplementedServiceTypes()
    {
        return ApplicationServices
           .Select(descriptor => descriptor.ImplementationType ?? descriptor.ServiceType)
           .Select(type => type.IsGenericType ? type.GetGenericTypeDefinition() : type)
           .ToList();
    }
}
