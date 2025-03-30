namespace AtendeLogo.Application.Contracts.Handlers;

public interface IGetQueryResultHandler<TResponse> : IRequestHandler<TResponse>
    where TResponse : IResponse
{
    public Task<Result<TResponse>> GetAsync(
         IQueryRequest<TResponse> query,
         CancellationToken cancellationToken = default);

    Task IApplicationHandler.HandleAsync(object handlerObject)
    {
        return GetAsync((IQueryRequest<TResponse>)handlerObject);
    }
}

public interface IGetQueryResultHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, TResponse>, IGetQueryResultHandler<TResponse>
    where TQuery : IQueryRequest<TResponse>
    where TResponse : IResponse
{
    Task<Result<TResponse>> HandleAsync(
        TQuery query,
        CancellationToken cancellationToken = default);

    Task<Result<TResponse>> IGetQueryResultHandler<TResponse>.GetAsync(
        IQueryRequest<TResponse> query,
        CancellationToken cancellationToken)
    {
        return HandleAsync((TQuery)query, cancellationToken);
    }
}
