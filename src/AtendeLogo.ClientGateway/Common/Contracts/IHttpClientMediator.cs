using System.Runtime.CompilerServices;
using AtendeLogo.Shared.Contracts;

namespace AtendeLogo.ClientGateway.Common.Contracts;

public interface IHttpClientMediator<T>
    where T : ICommunicationService
{
    #region Queries

    // GET
    Task<Result<TResponse>> GetAsync<TResponse>(
        IQueryRequest<TResponse> query,
        CancellationToken cancellationToken = default)
        where TResponse : IResponse;

    Task<Result<TResponse>> GetAsync<TResponse>(
        IQueryRequest<TResponse> query,
        string? route,
        CancellationToken cancellationToken = default)
        where TResponse : IResponse;

    // GET many
    Task<Result<IReadOnlyList<TResponse>>> GetManyAsync<TResponse>(
         IQueryRequest<TResponse> query,
         CancellationToken cancellationToken = default)
         where TResponse : IResponse;

    Task<Result<IReadOnlyList<TResponse>>> GetManyAsync<TResponse>(
         IQueryRequest<TResponse> query,
         string? route,
         CancellationToken cancellationToken = default)
         where TResponse : IResponse;

    #endregion

    #region Commands

    // POST
    Task<Result<TResponse>> PostAsync<TResponse>(
        ICommandRequest<TResponse> command,
        CancellationToken cancellationToken = default)
        where TResponse : IResponse;

    Task<Result<TResponse>> PostAsync<TResponse>(
        ICommandRequest<TResponse> command,
        string? route,
        CancellationToken cancellationToken = default)
        where TResponse : IResponse;

    // PUT
    Task<Result<TResponse>> PutAsync<TResponse>(
        ICommandRequest<TResponse> command,
        CancellationToken cancellationToken = default)
        where TResponse : IResponse;

    Task<Result<TResponse>> PutAsync<TResponse>(
        ICommandRequest<TResponse> command,
        string? route,
        CancellationToken cancellationToken = default)
        where TResponse : IResponse;

    // DELETE
    Task<Result<TResponse>> DeleteAsync<TResponse>(
        ICommandRequest<TResponse> command,
        CancellationToken cancellationToken = default)
        where TResponse : IResponse;

    Task<Result<TResponse>> DeleteAsync<TResponse>(
        ICommandRequest<TResponse> command,
        string? route,
        CancellationToken cancellationToken = default)
        where TResponse : IResponse;

    #endregion

    // VALIDATE
    Task<bool> IsValidAsync(
        string[] parameterNames,
        object[] parameterValues,
        CancellationToken cancellationToken = default,
        [CallerMemberName] string callerMethodName = "");
}

