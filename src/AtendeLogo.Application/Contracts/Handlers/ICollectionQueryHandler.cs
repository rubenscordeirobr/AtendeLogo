namespace AtendeLogo.Application.Contracts.Handlers;

internal interface ICollectionQueryHandler<TResponse> : IRequestHandler<TResponse>
    where TResponse : IResponse
{
    Task<IReadOnlyList<TResponse>> GetManyAsync(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default);

    Task IApplicationHandler.HandleAsync(object domainEvent)
    {
        return GetManyAsync((IQueryRequest<TResponse>)domainEvent);
    }
}

internal interface ICollectionQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, TResponse>, ICollectionQueryHandler<TResponse>
    where TQuery : IRequest<TResponse>
    where TResponse : IResponse
{
    Task<IReadOnlyList<TResponse>> GetManyAsync(
        TQuery request,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TResponse>> ICollectionQueryHandler<TResponse>.GetManyAsync(
       IRequest<TResponse> request,
       CancellationToken cancellationToken)
    {
        return GetManyAsync((TQuery)request, cancellationToken);
    }
}
