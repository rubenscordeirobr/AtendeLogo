namespace AtendeLogo.Application.Contracts.Handlers;

public interface IGetQueryResultHandler<TResponse> : IRequestHandler<TResponse>
    where TResponse : IResponse
{
    internal Task<Result<TResponse>> GetAsync(
         IQueryRequest<TResponse> request,
         CancellationToken cancellationToken = default);

    Task IApplicationHandler.HandleAsync(object domainEvent)
    {
        return GetAsync((IQueryRequest<TResponse>)domainEvent);
    }
}

public interface IGetQueryResultHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, TResponse>, IGetQueryResultHandler<TResponse>
    where TQuery : IQueryRequest<TResponse>
    where TResponse : IResponse
{
    Task<Result<TResponse>> HandleAsync(
        TQuery request,
        CancellationToken cancellationToken = default);

    Task<Result<TResponse>> IGetQueryResultHandler<TResponse>.GetAsync(
        IQueryRequest<TResponse> request,
        CancellationToken cancellationToken)
    {
        return HandleAsync((TQuery)request, cancellationToken);
    }
}
