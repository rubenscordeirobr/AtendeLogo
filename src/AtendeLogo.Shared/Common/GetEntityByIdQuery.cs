namespace AtendeLogo.UseCases.Common;

public abstract record GetEntityByIdQuery<TResponse> : QueryRequest<TResponse>
    where TResponse : ResponseBase
{
    public required Guid Id { get; init; }
}

