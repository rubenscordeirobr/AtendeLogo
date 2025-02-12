namespace AtendeLogo.Application.Contracts.Handlers;

public interface ISingleQueryResultHandler<TResponse> : IRequestHandler<TResponse>
    where TResponse : IResponse
{
    internal Task<Result<TResponse>> GetSingleAsync(
         IQueryRequest<TResponse> request,
         CancellationToken cancellationToken = default);

    Task IApplicationHandler.HandleAsync(object domainEvent)
    {
        return GetSingleAsync((IQueryRequest<TResponse>)domainEvent);
    }
}

public interface ISingleQueryResultHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, TResponse>, ISingleQueryResultHandler<TResponse>
    where TQuery : IQueryRequest<TResponse>
    where TResponse : IResponse
{
    Task<Result<TResponse>> HandleAsync(
        TQuery request,
        CancellationToken cancellationToken = default);

    Task<Result<TResponse>> ISingleQueryResultHandler<TResponse>.GetSingleAsync(
        IQueryRequest<TResponse> request,
        CancellationToken cancellationToken)
    {
        return HandleAsync((TQuery)request, cancellationToken);
    }
}
