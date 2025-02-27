using AtendeLogo.Presentation.Common.Exceptions;
using AtendeLogo.Presentation.Common;
using AtendeLogo.Shared.Contracts;
using AtendeLogo.Presentation.Common.Attributes;

namespace AtendeLogo.Application.UnitTests.Presentation.Common;

public class HttpMethodDescriptorTests
{
    [Fact]
    public void ValidDescriptor_ShouldExtractRouteAndQueryParameters()
    {
        // Arrange: obtain MethodInfo for GetItem.
        var method = typeof(TestEndpoint).GetMethod(nameof(TestEndpoint.GetItem));
        method.Should().NotBeNull();

        // Act
        var descriptor = new HttpMethodDescriptor(method!);

        // Assert
        // Expect one route parameter: "id"
        descriptor.RouteParameters.Should().HaveCount(1);
        descriptor.RouteParameters[0].Name.Should().Be("id");

        // Expect one query mapping: parameter "filter" mapped to query key "filter".
        descriptor.ParameterToQueryKeyMap.Should().HaveCount(1);
        var mapping = descriptor.ParameterToQueryKeyMap.First();
        mapping.Value.Should().Be("filter");
        mapping.Key.Name.Should().Be("filter");
    }

    [Fact]
    public void InvalidRouteTemplate_ShouldThrowRouteTemplateException()
    {
        // Arrange: method with route referencing "nonexistent".
        var method = typeof(TestEndpoint).GetMethod(nameof(TestEndpoint.GetItemInvalidRoute));
        method.Should().NotBeNull();

        // Act & Assert
        Action act = () => new HttpMethodDescriptor(method!);
        act.Should().Throw<RouteTemplateException>()
           .WithMessage("*does not match any parameter*")
           .And.Message.Should().Contain("{nonexistent}");
    }

    [Fact]
    public void InvalidQueryTemplateFormat_ShouldThrowRouteTemplateException()
    {
        // Arrange: method with invalid query template format.
        var method = typeof(TestEndpoint).GetMethod(nameof(TestEndpoint.GetItemInvalidQueryTemplate));
        method.Should().NotBeNull();

        // Act & Assert
        Action act = () => new HttpMethodDescriptor(method!);
        act.Should().Throw<RouteTemplateException>()
           .WithMessage("*the key-value pair*")
           .And.Message.Should()
           .Contain("invalidTemplate");
    }

    [Fact]
    public void QueryTemplateWithMissingParameter_ShouldThrowQueryTemplateException()
    {
        // Arrange: method where query template references a parameter "missing" not in signature.
        var method = typeof(TestEndpoint).GetMethod(nameof(TestEndpoint.GetItemMissingQuery));
        method.Should().NotBeNull();

        // Act & Assert
        Action act = () => new HttpMethodDescriptor(method!);
        act.Should().Throw<QueryTemplateException>()
           .WithMessage("*the parameter 'missing'*");
    }

    [Fact]
    public void ExtraParameterNotMapped_ShouldThrowHttpTemplateException()
    {
        // Arrange: method with an extra parameter "sort" not mapped in route or query.
        var method = typeof(TestEndpoint).GetMethod(nameof(TestEndpoint.GetItemExtraParameter));
        method.Should().NotBeNull();

        // Act & Assert
        Action act = () => new HttpMethodDescriptor(method!);
        act.Should().Throw<HttpTemplateException>()
           .WithMessage("*not mapped*")
           .And.Message.Should().Contain("sort");
    }

    [Fact]
    public void ParameterMappedInBothRouteAndQuery_ShouldThrowHttpTemplateException()
    {
        // Arrange: method where parameter "id" is mapped in both route and query.
        var method = typeof(TestEndpoint).GetMethod(nameof(TestEndpoint.GetItemDuplicateMapping));
        method.Should().NotBeNull();

        // Act & Assert
        Action act = () => new HttpMethodDescriptor(method!);
        act.Should().Throw<HttpTemplateException>()
           .WithMessage("*mapped in both the route and query*")
           .And.Message.Should().Contain("id");
    }

    [Fact]
    public void SingleParameterImplementingIRequest_ShouldBypassValidation()
    {
        // Arrange: method with a single parameter that implements IRequest<>.
        var method = typeof(TestEndpoint).GetMethod(nameof(TestEndpoint.CreateItem));
        method.Should().NotBeNull();

        // Act & Assert: no exception should be thrown.
        Action act = () => new HttpMethodDescriptor(method!);
        act.Should().NotThrow();
    }

    [Fact]
    public void Method_ShouldCreateQueryTemplate()
    {
        // Arrange
        var method = typeof(TestEndpoint).GetMethod(nameof(TestEndpoint.Parameters));
        method.Should().NotBeNull();
        
        // Act
        var descriptor = new HttpMethodDescriptor(method!);
        var result = descriptor.QueryTemplate;

        // Assert
        result.Should()
            .Be("parameter1={parameter1}&parameter2={parameter2}&parameter3={parameter3}");
    }

    [Fact]
    public void Method_ShouldBeBodyParameter()
    {
        // Arrange
        var method = typeof(TestEndpoint).GetMethod(nameof(TestEndpoint.CreateItem));
        method.Should().NotBeNull();
        
        // Act
        var descriptor = new HttpMethodDescriptor(method!);
        var result = descriptor.IsBodyParameter;
        
        // Assert
        result.Should().BeTrue();
    }

}

// Test endpoint used for testing.
public class TestEndpoint
{
    // Valid: route "items/{id}" and query "filter={filter}"
    [HttpGet("items/{id}", "filter={filter}")]
    public void GetItem(int id, string filter) { }

    // Invalid route: route references a parameter ("nonexistent") not in the method signature.
    [HttpPost("items/{nonexistent}")]
    public void GetItemInvalidRoute(int id) { }

    // Invalid query template format: missing '=' in the query template.
    [HttpGet("items/{id}", "invalidTemplate")]
    public void GetItemInvalidQueryTemplate(int id) { }

    // Query template references non-existent parameter: query "filter={missing}"
    [HttpGet("items/{id}", "filter={missing}")]
    public void GetItemMissingQuery(int id) { }

    // Extra parameter not mapped: method has two parameters, but route only maps "id".
    [HttpPost("items/{id}")]
    public void GetItemExtraParameter(int id, string sort) { }

    [HttpPost("items/{id}")]
    public void GetItemDefaultParameter(int id, bool defaultValue = true) { }

    // Parameter mapped in both route and query: route maps "id" and query maps "id={id}".
    [HttpGet("items/{id}", "id={id}")]
    public void GetItemDuplicateMapping(int id) { }

    // Valid scenario: single parameter implementing IRequest<> should bypass additional validation.
    [HttpPost("items")]
    public void CreateItem(TestRequest request) { }
     
    [HttpGet]
    public int Parameters(int parameter1, double parameter2, string parameter3)
    {
        return 0;
    }
     
}

public class TestRequest : IRequest<IResponse>
{
}

