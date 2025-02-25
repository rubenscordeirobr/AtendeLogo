using System.Net;
using AtendeLogo.Presentation.Common.Enums;

namespace AtendeLogo.Presentation.Common.Attributes;

public class HttpGetAttribute : HttpMethodAttribute
{
    public override HttpVerb HttpVerb
        => HttpVerb.Get;
     
    public HttpGetAttribute(
        string routeTemplate = "",
        string queryTemplate = "")
        : this(HttpStatusCode.OK, routeTemplate, queryTemplate) { }

    public HttpGetAttribute(
        HttpStatusCode httpStatusCode,
        string routeTemplate = "",
        string queryTemplate = "")
        : base(httpStatusCode, routeTemplate, queryTemplate)
    {

    }
}
