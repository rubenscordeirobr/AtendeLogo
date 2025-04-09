
namespace AtendeLogo.ClientGateway.Common.Abstractions;

public interface IHttpClientProvider
{
    HttpClient GetHttpClient<T>();
}
