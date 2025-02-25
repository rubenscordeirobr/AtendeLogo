using System.Net;
using AtendeLogo.Presentation.Common.Enums;

namespace AtendeLogo.Presentation.Common.Attributes;

public class HttpPutAttribute : HttpMethodAttribute
{
    public override HttpVerb HttpVerb
        => HttpVerb.Put;

    public HttpPutAttribute(string routeTemplate = "") : base(routeTemplate) { }

    public HttpPutAttribute(HttpStatusCode httpStatusCode, string template = "")
        : base(httpStatusCode, template) { }
}
