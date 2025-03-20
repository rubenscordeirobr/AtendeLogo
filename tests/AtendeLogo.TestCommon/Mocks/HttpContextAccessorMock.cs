using Microsoft.AspNetCore.Http;

namespace AtendeLogo.TestCommon.Mocks;

public class HttpContextAccessorMock : IHttpContextAccessor
{
    public HttpContext? HttpContext { get; set; } = new DefaultHttpContext();
}
