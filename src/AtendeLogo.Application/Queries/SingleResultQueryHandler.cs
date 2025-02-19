using AtendeLogo.Application.Contracts.Handlers;

namespace AtendeLogo.Application.Queries;

public abstract class SingleResultQueryHandler<TQuery, TResponse> 
    : ISingleQueryResultHandler<TQuery, TResponse>
    where TQuery : IQueryRequest<TResponse>
    where TResponse : IResponse
{
    public abstract Task<Result<TResponse>> HandleAsync(
        TQuery query,
        CancellationToken cancellationToken = default);
}
