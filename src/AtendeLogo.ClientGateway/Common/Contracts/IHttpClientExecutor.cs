using AtendeLogo.ClientGateway.Common.Factories;

namespace AtendeLogo.ClientGateway.Common.Contracts;

public interface IHttpClientExecutor
{
    Uri BaseAddress { get; }

    Task<Result<T>> SendAsync<T>(
        HttpRequestMessageFactory messageFactory,
        CancellationToken cancellationToken)
        where T : notnull;
}

