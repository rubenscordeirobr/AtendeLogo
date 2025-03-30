using System.Net.Http.Json;
using System.Text.Json;
using AtendeLogo.ClientGateway.Common.Helpers;
using AtendeLogo.Common.Utils;

namespace AtendeLogo.ClientGateway.Common.Factories;

public class JsonMessageFactory : HttpRequestMessageFactory
{
    private readonly object _value;
    public JsonMessageFactory(
        HttpMethod method,
        Uri requestUri,
        object content)
        : base(method, requestUri)
    {
        _value = content;
        HttpClientHelper.ThrowIfMethodNotAllowBody(method, requestUri);
    }
    protected override Task<HttpRequestMessage> CreateMessageAsync()
    {
        var options = JsonSerializerOptions.Web;
        JsonUtils.EnableIndentationInDevelopment(options);

        var jsonContent = JsonContent.Create(_value, options: options);
        var message = new HttpRequestMessage(Method, RequestUri)
        {
            Content = jsonContent
        };
       
        return Task.FromResult(message);
    }

}
