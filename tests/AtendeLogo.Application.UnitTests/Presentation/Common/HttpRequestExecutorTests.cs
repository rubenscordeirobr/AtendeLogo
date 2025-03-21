using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using AtendeLogo.Presentation.Common;
using AtendeLogo.Presentation.Common.Attributes;

namespace AtendeLogo.Application.UnitTests.Presentation.Common;

public class HttpRequestExecutorTests
{
    private HttpMethodDescriptor CreateDescriptor(MethodInfo method)
    {
        return new HttpMethodDescriptor(method);
    }

    private HttpContext CreateHttpContext(IServiceProvider serviceProvider)
    {
        var context = new DefaultHttpContext();
        context.RequestServices = serviceProvider;
        return context;
    }

    private IServiceProvider CreateServiceProvider(
        ApiEndpointBase? endpointInstance = null,
        ILogger<HttpRequestExecutor>? logger = null)
    {
        logger ??= Mock.Of<ILogger<HttpRequestExecutor>>();
        var serviceCollection = new ServiceCollection();
        serviceCollection
            .AddSingleton(logger);

        if (endpointInstance != null)
        {
            serviceCollection.AddSingleton(endpointInstance.GetType(), endpointInstance);
        }
        return serviceCollection.BuildServiceProvider();
    }
     
    [Fact]
    public async Task HandleAsync_WhenMethodExecutesSuccessfully_ShouldReturnOk()
    {
        // Arrange
        var method = typeof(TestValidEndpoint).GetMethod(nameof(TestValidEndpoint.GetItem));
        var descriptor = CreateDescriptor(method!);
        var endpointInstance = new TestValidEndpoint();
        var serviceProvider = CreateServiceProvider(endpointInstance);
        var context = CreateHttpContext(serviceProvider);

        var handler = new HttpRequestExecutor(context, typeof(TestValidEndpoint), descriptor);

        // Act
        await handler.HandleAsync();

        // Assert
        context.Response.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [Fact]
    public async Task HandleAsync_WhenMethodThrowsException_ShouldReturnInternalServerError()
    {
        // Arrange
        var method = typeof(TestValidEndpoint).GetMethod(nameof(TestValidEndpoint.ThrowException));
        var descriptor = CreateDescriptor(method!);
        var endpointInstance = new TestValidEndpoint();
        var serviceProvider = CreateServiceProvider(endpointInstance);
        var context = CreateHttpContext(serviceProvider);

        var handler = new HttpRequestExecutor(context, typeof(TestValidEndpoint), descriptor);

        // Act
        await handler.HandleAsync();

        // Assert
        context.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task HandleAsync_WhenRequestIsCanceled_ShouldReturnRequestAborted()
    {
        // Arrange
        var method = typeof(TestValidEndpoint).GetMethod(nameof(TestValidEndpoint.LongRunningTask));
        var descriptor = CreateDescriptor(method!);
        var endpointInstance = new TestValidEndpoint();
        var serviceProvider = CreateServiceProvider(endpointInstance);
        var context = CreateHttpContext(serviceProvider);

        using var cts = new CancellationTokenSource();
        context.RequestAborted = cts.Token;

        var handler = new HttpRequestExecutor(context, typeof(TestValidEndpoint), descriptor);

        // Simulate request cancellation before execution
        await cts.CancelAsync();
        
        // Act
        await handler.HandleAsync();

        // Assert
        context.Response.StatusCode.Should().Be((int)ExtendedHttpStatusCode.RequestAborted);
    }
     
    [Fact]
    public async Task GetResponseResultAsync_WhenInvalidEndpoint_ShouldReturnError()
    {
        // Arrange
        var method = typeof(TestInvalidEndpoint).GetMethod(nameof(TestInvalidEndpoint.Test));
        var descriptor = CreateDescriptor(method!);
        var context = CreateHttpContext(CreateServiceProvider(null));

        var handler = new HttpRequestExecutor(context, typeof(TestInvalidEndpoint), descriptor);

        // Act
        var response = await handler.GetResponseResultAsync();

        // Assert
        response.StatusCode
            .Should().Be((int)HttpStatusCode.InternalServerError);
        response.ErrorResponse!.Code.Should().Be("HttpRequestExecutor.InvalidEndPointType");
    }

    [Fact]
    public async Task GetResponseResultAsync_WhenMethodThrowsException_ShouldReturnError()
    {
        // Arrange
        var method = typeof(TestValidEndpoint).GetMethod(nameof(TestValidEndpoint.ThrowException));
        var descriptor = CreateDescriptor(method!);
        var endpointInstance = new TestValidEndpoint();
        var serviceProvider = CreateServiceProvider(endpointInstance);
        var context = CreateHttpContext(serviceProvider);
        var handler = new HttpRequestExecutor(context, typeof(TestValidEndpoint), descriptor);

        // Act
        var response = await handler.GetResponseResultAsync();

        // Assert
        response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        response.ErrorResponse!.Code.Should().Be("HttpRequestExecutor.ErrorInvokingMethod");
    }
  
}
 
// Mock Test Endpoint
public class TestValidEndpoint : ApiEndpointBase
{
    [HttpGet("items/{id}")]
    public int GetItem(int id = 10) => id;

    [HttpGet("test-exception/{id}")]
    public void ThrowException()
        => throw new InvalidOperationException("Test exception");

    [HttpPost("items")]
    public async Task LongRunningTask()
    {
        await Task.Delay(5000); // Simulating long-running request
    }
}

public class TestInvalidEndpoint 
{
    [HttpGet("test")]
    public int Test() => 1;
    public TestInvalidEndpoint(bool test)
    {

    }
}
