using AtendeLogo.Application.Contracts.Handlers;

namespace AtendeLogo.Application.Queries;

public abstract class CollectionQueryHandler<TRequest, TResponse>
    : ICollectionQueryHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResponse
{
    public Task<IReadOnlyList<TResponse>> GetManyAsync(
        TRequest query, 
        CancellationToken cancellationToken = default)
    {
        return HandleAsync(query, cancellationToken);
    }

    public abstract Task<IReadOnlyList<TResponse>> HandleAsync(
        TRequest request, 
        CancellationToken cancellationToken = default);
}
