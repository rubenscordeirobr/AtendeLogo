namespace AtendeLogo.ClientGateway.Common.Factories;

public class FormMessageFactory : HttpRequestMessageFactory
{
    private readonly IEnumerable<KeyValuePair<string, string>> _keyValuePairs;
    public FormMessageFactory(
        HttpMethod method,
        Uri requestUri,
        IEnumerable<KeyValuePair<string, string>> keyValuePairs)
        : base(method, requestUri)
    {
        _keyValuePairs = keyValuePairs;
    }
    protected override Task<HttpRequestMessage> CreateMessageAsync()
    {
        var content = new FormUrlEncodedContent(_keyValuePairs);
        var message = new HttpRequestMessage(Method, RequestUri)
        {
            Content = content
        };
        return Task.FromResult(message);
    }
}
