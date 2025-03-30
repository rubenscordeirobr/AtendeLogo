namespace AtendeLogo.ClientGateway.Common.Factories;

public class NoContentMessageFactory : HttpRequestMessageFactory
{
    public NoContentMessageFactory(
        HttpMethod method,
        Uri requestUri)
        : base(method, requestUri)
    {
    }

    protected override Task<HttpRequestMessage> CreateMessageAsync()
    {
        return Task.FromResult(new HttpRequestMessage(Method, RequestUri));
    }
}
