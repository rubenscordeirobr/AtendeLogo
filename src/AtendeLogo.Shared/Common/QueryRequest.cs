namespace AtendeLogo.UseCases.Common;

public abstract record QueryRequest<TResponse>
    : Request<TResponse>, IQueryRequest<TResponse>
    where TResponse : ResponseBase
{
}
