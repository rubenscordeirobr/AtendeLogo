namespace AtendeLogo.Application.Contracts.Mediators;

public interface ICommandMediator
{
    Task<Result<TResponse>> RunAsync<TResponse>(
        ICommandRequest<TResponse> command,
        CancellationToken cancellationToken = default)
        where TResponse : IResponse;
}
