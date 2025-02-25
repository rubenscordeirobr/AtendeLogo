using System.Net;
using AtendeLogo.Presentation.Common.Enums;

namespace AtendeLogo.Presentation.Common.Attributes;

public class HttpPostAttribute : HttpMethodAttribute
{
    public override HttpVerb HttpVerb
        => HttpVerb.Post;

    public HttpPostAttribute(string routeTemplate = "") : base(routeTemplate) { }

    public HttpPostAttribute(HttpStatusCode httpStatusCode, string template = "")
        : base(httpStatusCode, template) { }
}
