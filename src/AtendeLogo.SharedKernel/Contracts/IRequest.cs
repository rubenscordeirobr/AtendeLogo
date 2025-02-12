namespace AtendeLogo.Shared.Contracts;

public interface IRequest<TResponse>
    where TResponse : IResponse
{
}

public interface IQueryRequest<TResponse> : IRequest<TResponse>
    where TResponse : IResponse
{
}

public interface ICommandRequest<TResponse> : IRequest<TResponse>
    where TResponse : IResponse
{
    Guid ClientRequestId { get; }
}
