using AtendeLogo.Application.Contracts.Handlers;
using AtendeLogo.Application.UnitTests.Mocks;
using AtendeLogo.Shared.Contracts;

namespace AtendeLogo.ArchitectureTests;

public class RequestValidationTests
    : IClassFixture<ApplicationAssemblyContext>,
    IClassFixture<AnonymousServiceProviderMock>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IReadOnlyDictionary<Type, Type> _requestTypeToHandlerTypeMap;
    private readonly ITestOutputHelper _output;

    public RequestValidationTests(
        ApplicationAssemblyContext assemblyContext,
        AnonymousServiceProviderMock serviceProvider,
        ITestOutputHelper output)
    {
        _serviceProvider = serviceProvider;
        _requestTypeToHandlerTypeMap = assemblyContext.RequestTypeToHandlerTypeMap;
        _output = output;
    }

    public static IEnumerable<object[]> RequestTypes
    {
        get
        {
            var assemblyContext = new ApplicationAssemblyContext();
            var requestTypes = assemblyContext.RequestTypes;
            return requestTypes.Select(type => new object[] { type });
        }
    }
     
    [Theory]
    [MemberData(nameof(RequestTypes))]
    public void Request_Should_Have_Handler_Registered(Type type)
    {
        // Act
        var handlerType = _requestTypeToHandlerTypeMap.GetValueOrDefault(type);
        var responseType = type.GetGenericArgumentFromInterfaceDefinition(typeof(IRequest<>));
        var handlerServiceType = typeof(IRequestHandler<,>)
             .MakeGenericType(type, responseType);

        var handlerService = _serviceProvider.GetService(handlerServiceType);

        // Assert
        handlerType
            .Should()
            .NotBeNull($"Request {type.Name} should have a RequestHandler.");

        handlerService
            .Should()
            .NotBeNull($"RequestHandler {handlerType!.Name} should be registered.");

        _output.WriteLine($"Request {type.Name} has a registered RequestHandler {handlerType.Name}.");
    }
}

