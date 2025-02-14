using AtendeLogo.Application.Contracts.Handlers;

namespace AtendeLogo.Application.Queries;

public abstract class SingleResultQueryHandler<TRequest, TResponse> 
    : ISingleQueryResultHandler<TRequest, TResponse>
    where TRequest : IQueryRequest<TResponse>
    where TResponse : IResponse
{
    public abstract Task<Result<TResponse>> HandleAsync(
        TRequest request,
        CancellationToken cancellationToken = default);
}
