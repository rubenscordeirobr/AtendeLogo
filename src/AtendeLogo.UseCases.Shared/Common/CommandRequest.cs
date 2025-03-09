namespace AtendeLogo.UseCases.Common;

public abstract record CommandRequest<TResponse>
    : IRequest<TResponse>, ICommandRequest<TResponse>
    where TResponse : IResponse
{
    public required Guid ClientRequestId { get; init; }
}
