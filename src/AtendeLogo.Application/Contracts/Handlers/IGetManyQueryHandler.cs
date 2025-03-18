namespace AtendeLogo.Application.Contracts.Handlers;

internal interface IGetManyQueryHandler<TResponse> : IRequestHandler<TResponse>
    where TResponse : IResponse
{
    Task<Result<IReadOnlyList<TResponse>>> GetManyAsync(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default);

    Task IApplicationHandler.HandleAsync(object domainEvent)
    {
        return GetManyAsync((IQueryRequest<TResponse>)domainEvent);
    }
}

internal interface IGetManyQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, TResponse>, IGetManyQueryHandler<TResponse>
    where TQuery : IRequest<TResponse>
    where TResponse : IResponse
{
    Task<Result<IReadOnlyList<TResponse>>> GetManyAsync(
        TQuery request,
        CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<TResponse>>> IGetManyQueryHandler<TResponse>.GetManyAsync(
       IRequest<TResponse> request,
       CancellationToken cancellationToken)
    {
        return GetManyAsync((TQuery)request, cancellationToken);
    }
}
