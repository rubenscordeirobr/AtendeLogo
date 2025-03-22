using System.Net;

namespace AtendeLogo.Presentation.Common.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class HttpPostFormAttribute : HttpMethodAttribute
{
    public override HttpVerb HttpVerb
        => HttpVerb.Post;
    public HttpPostFormAttribute(string operationTemplate = "")
        : base(HttpStatusCode.OK, "", operationTemplate) { }
}
