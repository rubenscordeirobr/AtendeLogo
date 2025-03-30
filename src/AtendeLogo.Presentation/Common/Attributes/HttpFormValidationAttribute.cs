using System.Net;

namespace AtendeLogo.Presentation.Common.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class HttpFormValidationAttribute : HttpMethodAttribute
{
    public override HttpVerb HttpVerb
        => HttpVerb.Post;

    public HttpFormValidationAttribute(string operationTemplate = "")
        : base(HttpStatusCode.OK, "", operationTemplate) { }
}
