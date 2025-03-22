using System.Net;

namespace AtendeLogo.Presentation.Common.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class HttpDeleteAttribute : HttpMethodAttribute
{
    public override HttpVerb HttpVerb
        => HttpVerb.Delete;
    public HttpDeleteAttribute(string routeTemplate = "") : base(routeTemplate) { }

    public HttpDeleteAttribute(HttpStatusCode successStatusCode, string routeTemplate = "")
        : base(successStatusCode, routeTemplate) { }
}
