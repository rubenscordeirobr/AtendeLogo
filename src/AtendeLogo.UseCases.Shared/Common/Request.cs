namespace AtendeLogo.UseCases.Common;

public abstract record Request<TResponse> : IRequest<TResponse>
    where TResponse : ResponseBase
{
}
