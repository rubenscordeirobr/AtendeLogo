using Microsoft.AspNetCore.Http;

namespace AtendeLogo.Application.UnitTests.Mocks;

public class HttpContextAccessorMock : IHttpContextAccessor
{
    public HttpContext? HttpContext { get; set; } = new DefaultHttpContext();
}
