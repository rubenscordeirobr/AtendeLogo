namespace AtendeLogo.Application.Contracts.Mediators;

public interface IQueryMediador
{
    Task<Result<TResponse>> GetSingleAsync<TResponse>(
        IQueryRequest<TResponse> query,
        CancellationToken cancellationToken = default)
            where TResponse : IResponse;

    Task<IReadOnlyList<TResponse>> GetManyAsync<TResponse>(
        IQueryRequest<TResponse> request,
        CancellationToken cancellationToken = default)
            where TResponse : IResponse;

}
