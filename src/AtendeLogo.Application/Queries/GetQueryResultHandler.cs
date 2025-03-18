using AtendeLogo.Application.Contracts.Handlers;

namespace AtendeLogo.Application.Queries;

public abstract class GetQueryResultHandler<TQuery, TResponse> 
    : IGetQueryResultHandler<TQuery, TResponse>
    where TQuery : IQueryRequest<TResponse>
    where TResponse : IResponse
{
    public abstract Task<Result<TResponse>> HandleAsync(
        TQuery query,
        CancellationToken cancellationToken = default);
}
