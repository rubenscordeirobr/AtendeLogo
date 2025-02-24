namespace AtendeLogo.UseCases.Common;

public abstract record GetEntityByIdQuery<TResponse>(Guid Id) : QueryRequest<TResponse>
    where TResponse : ResponseBase;
