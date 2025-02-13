using AtendeLogo.Application.Contracts.Handlers;
using AtendeLogo.Application.Contracts.Services;
using AtendeLogo.Application.Exceptions;
using AtendeLogo.Common.Extensions;

namespace AtendeLogo.Application.Mediatores;

internal partial class RequestMediator : IRequestMediator
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ICommandTrackingService _tackingService;

    public RequestMediator(
        IServiceProvider serviceProvider,
        ICommandTrackingService requestTrackingService)
    {
        _serviceProvider = serviceProvider;
        _tackingService = requestTrackingService;
    }

    #region Command

    public Task<Result<TResponse>> RunAsync<TResponse>(
         ICommandRequest<TResponse> command,
         CancellationToken cancellationToken = default)
         where TResponse : IResponse
    {
        return RunCommandAsync(command, cancellationToken);
    }

    private async Task<Result<TResponse>> RunCommandAsync<TResponse>(
        ICommandRequest<TResponse> command,
        CancellationToken cancellationToken)
        where TResponse : IResponse
    {
        var validator = new CommandValidorExecutor<TResponse>(_serviceProvider, command);

        var validationResult = await validator.ValidateAsync(cancellationToken);
        if (validationResult.IsFailure)
        {
            return Result.Failure<TResponse>(validationResult.Error);
        }

        var clienteRequestId = command.ClientRequestId;
        if (await _tackingService.ExistsAsync(clienteRequestId, cancellationToken))
        {
            var existingResult = await _tackingService
                .TryGetResultAsync<TResponse>(clienteRequestId, cancellationToken);

            if (existingResult?.IsSuccess == true)
            {
                return Result.Success(existingResult.Value);
            }
        }

        var handler = GetRequestHandler<ICommandHandler<TResponse>, TResponse>(command);
        var result = await handler.RunAsync(command, cancellationToken);
        if (result == null)
        {
            var message = $"Handler not found for command {command.GetType().GetQualifiedName()}";
            throw new RequestHandlerNotFoundException(message);
        }

        if (result.IsSuccess)
        {
            await _tackingService.TrackAsync(clienteRequestId, result);
        }
        return result;
    }

    #endregion

    #region Query

    public Task<Result<TResponse>> GetSingleAsync<TResponse>(
      IQueryRequest<TResponse> queryReqyest,
      CancellationToken cancellationToken = default)
          where TResponse : IResponse
    {
        var handler = GetRequestHandler<ISingleQueryResultHandler<TResponse>, TResponse>(queryReqyest);
        return handler.GetSingleAsync(queryReqyest, cancellationToken);
    }

    public Task<IReadOnlyList<TResponse>> GetManyAsync<TResponse>(
        IQueryRequest<TResponse> queryReqyest,
        CancellationToken cancellationToken = default)
            where TResponse : IResponse
    {
        var handler = GetRequestHandler<ICollectionQueryHandler<TResponse>, TResponse>(queryReqyest);
        return handler.GetManyAsync(queryReqyest, cancellationToken);
    }


    #endregion

    #region Private

    private TRequsetHandler GetRequestHandler<TRequsetHandler, TResponse>(
          IRequest<TResponse> request)
          where TRequsetHandler : class, IRequestHandler<TResponse>
          where TResponse : IResponse
    {
        var requestType = request.GetType();
        var responseType = typeof(TResponse);

        var handlerType = typeof(IRequestHandler<,>)
            .MakeGenericType(requestType, responseType);

        var handler = _serviceProvider.GetService(handlerType);
        if (handler == null)
        {
            throw new RequestHandlerNotFoundException(
                $"Handler not found not registered for Request," +
                $" Request: {requestType.Name} and" +
                $" Response {responseType.Name} not registered" +
                $"Handler type {typeof(TRequsetHandler).Name}");
        }

        if (handler is TRequsetHandler requsetHandler)
        {
            return requsetHandler;
        }

        throw new InvalidCastException(
            $"Could not cast the request handler {handler.GetType().GetQualifiedName()} to {typeof(TRequsetHandler).GetQualifiedName()}");
    }

    #endregion
}
