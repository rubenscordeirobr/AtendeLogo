using System.Net;
using AtendeLogo.Presentation.Common.Enums;

namespace AtendeLogo.Presentation.Common.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public abstract class HttpMethodAttribute : Attribute
{
    public string RouteTemplate { get; }
    public string QueryTemplate { get; }

    public HttpStatusCode SuccessStatusCode { get; }

    public abstract HttpVerb HttpVerb { get; }

    public string[] HttpMethods
        => [HttpVerb.ToString().ToUpper()];
      
    protected HttpMethodAttribute(string routeTemplate)
        : this(HttpStatusCode.OK, routeTemplate)
    {

    }

    protected HttpMethodAttribute(
        HttpStatusCode httpStatusCode,
        string routeTemplate,
        string queryTemplate = "")
    {
        RouteTemplate = routeTemplate;
        SuccessStatusCode = httpStatusCode;
        QueryTemplate = queryTemplate;
    }
}
