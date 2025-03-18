namespace AtendeLogo.Application.Contracts.Mediators;

public interface IQueryMediador
{
    Task<Result<TResponse>> GetAsync<TResponse>(
        IQueryRequest<TResponse> query,
        CancellationToken cancellationToken = default)
            where TResponse : IResponse;

    Task<Result<IReadOnlyList<TResponse>>> GetManyAsync<TResponse>(
        IQueryRequest<TResponse> request,
        CancellationToken cancellationToken = default)
            where TResponse : IResponse;

}
