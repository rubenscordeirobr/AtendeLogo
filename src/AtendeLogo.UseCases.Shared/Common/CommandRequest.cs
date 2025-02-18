namespace AtendeLogo.UseCases.Common;

public abstract record CommandRequest<TResponse>
    : Request<TResponse>, ICommandRequest<TResponse>
    where TResponse : ResponseBase
{
    public required Guid ClientRequestId { get; init; }
}
