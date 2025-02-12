namespace AtendeLogo.UseCases.Common;

public abstract record CommandRequest<TResponse>(Guid ClientRequestId)
    : Request<TResponse>, ICommandRequest<TResponse>
    where TResponse : ResponseBase;
 
