using System.Net;

namespace AtendeLogo.Presentation.Common.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class HttpPutAttribute : HttpMethodAttribute
{
    public override HttpVerb HttpVerb
        => HttpVerb.Put;

    public HttpPutAttribute(string routeTemplate = "") : base(routeTemplate) { }

    public HttpPutAttribute(HttpStatusCode successStatusCode, string routeTemplate = "")
        : base(successStatusCode, routeTemplate) { }
}
