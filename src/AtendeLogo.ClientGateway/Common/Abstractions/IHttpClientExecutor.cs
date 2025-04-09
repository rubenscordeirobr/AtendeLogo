using AtendeLogo.ClientGateway.Common.Factories;

namespace AtendeLogo.ClientGateway.Common.Abstractions;

public interface IHttpClientExecutor : IDisposable
{
    Task<Result<T>> SendAsync<T>(
        HttpRequestMessageFactory messageFactory,
        CancellationToken cancellationToken)
        where T : notnull;
}

