using AtendeLogo.Application.Contracts.Handlers;

namespace AtendeLogo.Application.Queries;

public abstract class GetManyQueryHandler<TRequest, TResponse>
    : IGetManyQueryHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResponse
{
    public Task<Result<IReadOnlyList<TResponse>>> GetManyAsync(
        TRequest query, 
        CancellationToken cancellationToken = default)
    {
        return HandleAsync(query, cancellationToken);
    }

    public abstract Task<Result<IReadOnlyList<TResponse>>> HandleAsync(
        TRequest request, 
        CancellationToken cancellationToken = default);

    protected Result<IReadOnlyList<TResponse>> Success(
        List<TResponse> list)
    {
        return Result.Success<IReadOnlyList<TResponse>>(list);
    }
}
