using AtendeLogo.Presentation.Common.Exceptions;
using AtendeLogo.Presentation.Common;
using Moq;
using Microsoft.AspNetCore.Routing;
using AtendeLogo.Presentation.Extensions;
using AtendeLogo.Presentation.Common.Attributes;

namespace AtendeLogo.Application.UnitTests.Presentation.Common;

public class EndpointBuilderExtensionsTests
{

    [Fact]
    public void Map_ShouldThrow_WhenDuplicateHttpTemplateExists()
    {
        // Arrange
        var app = Mock.Of<IEndpointRouteBuilder>();
        var endpointType = typeof(TestDuplicateEndpoint);

        // Act
        Action act = () => app.MapEndpointType(endpointType);

        // Assert
        act.Should().Throw<DuplicateEndpointException>();
    }

    [Fact]
    public void Map_ShouldThrow_WhenEndpointAttributeIsMissiong()
    {
        // Arrange
        var app = Mock.Of<IEndpointRouteBuilder>();
        var endpointType = typeof(TestWithoutAttributeEndpoint);

        // Act
        Action act = () => app.MapEndpointType(endpointType);

        // Assert
        act.Should().Throw<EndpointAttributeException>();
    }

}

public class TestWithoutAttributeEndpoint : ApiEndpointBase
{

}

[EndPoint("/")]
public class TestDuplicateEndpoint : ApiEndpointBase
{
    [HttpGet("items/{id}")]
    public int GetItem(int id = 10) => id;

    [HttpGet("items/{id}")]
    public int GetItem2(int id = 10) => id;
}
