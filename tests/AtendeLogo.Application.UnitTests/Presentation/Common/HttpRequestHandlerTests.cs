using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using AtendeLogo.Presentation.Common;
using AtendeLogo.Presentation.Common.Enums;
using AtendeLogo.Presentation.Common.Attributes;
using AtendeLogo.Shared.Contracts;
namespace AtendeLogo.Application.UnitTests.Presentation.Common;
public class HttpRequestHandlerTests
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
        ILogger<HttpRequestHandler>? logger = null)
    {
        logger ??= Mock.Of<ILogger<HttpRequestHandler>>();
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

        var handler = new HttpRequestHandler(context, typeof(TestValidEndpoint), descriptor);

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

        var handler = new HttpRequestHandler(context, typeof(TestValidEndpoint), descriptor);

        // Act
        await handler.HandleAsync();

        // Assert
        context.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task HandleAsync_WhenRequestIsCancelled_ShouldReturnRequestAborted()
    {
        // Arrange
        var method = typeof(TestValidEndpoint).GetMethod(nameof(TestValidEndpoint.LongRunningTask));
        var descriptor = CreateDescriptor(method!);
        var endpointInstance = new TestValidEndpoint();
        var serviceProvider = CreateServiceProvider(endpointInstance);
        var context = CreateHttpContext(serviceProvider);
        var cts = new CancellationTokenSource();
        context.RequestAborted = cts.Token;

        var handler = new HttpRequestHandler(context, typeof(TestValidEndpoint), descriptor);

        // Simulate request cancellation before execution
        cts.Cancel();

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

        var handler = new HttpRequestHandler(context, typeof(TestInvalidEndpoint), descriptor);

        // Act
        var response = await handler.GetResponseResultAsync();

        // Assert
        response.StatusCode
            .Should().Be((int)HttpStatusCode.InternalServerError);
        response.ErroResult!.Code.Should().Be("HttpRequestHandler.InvalidEndPointType");
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
        var handler = new HttpRequestHandler(context, typeof(TestValidEndpoint), descriptor);

        // Act
        var response = await handler.GetResponseResultAsync();

        // Assert
        response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        response.ErroResult!.Code.Should().Be("HttpRequestHandler.ErrorInvokingMethod");
    }
}

// Mock Test Endpoint
public class TestValidEndpoint : ApiEndpointBase
{
    [HttpGet("items/{id}")]
    public int GetItem(int id = 10) => id;

    [HttpGet("items/{id}")]
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
