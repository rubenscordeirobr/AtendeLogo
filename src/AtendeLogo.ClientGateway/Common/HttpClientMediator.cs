using System.Runtime.CompilerServices;
using AtendeLogo.ClientGateway.Common.Factories;
using AtendeLogo.ClientGateway.Common.Helpers;
using AtendeLogo.Common.Exceptions;
using AtendeLogo.Common.Helpers;
using AtendeLogo.Shared.Contracts;

namespace AtendeLogo.ClientGateway.Common;

public class HttpClientMediator<T> : IHttpClientMediator<T>
    where T : ICommunicationService
{
    private readonly IHttpClientExecutor _executor;
    private readonly ValidationResultCache _validationCache = new();
    private readonly string _baseRoute;

    public HttpClientMediator(IHttpClientExecutor executor)
    {
        _executor = executor;
        _baseRoute = RouteBinder.GetRoute<T>();
    }

    #region Queries

    public Task<Result<TResponse>> GetAsync<TResponse>(
      IQueryRequest<TResponse> query,
      CancellationToken cancellationToken = default)
      where TResponse : IResponse
    {
        return GetAsyncInternal<TResponse>(null, query, cancellationToken);
    }

    public Task<Result<TResponse>> GetAsync<TResponse>(
        IQueryRequest<TResponse> query,
        string? route,
        CancellationToken cancellationToken = default)
        where TResponse : IResponse
    {
        return GetAsyncInternal<TResponse>(route, query, cancellationToken);
    }

    public Task<Result<IReadOnlyList<TResponse>>> GetManyAsync<TResponse>(
      IQueryRequest<TResponse> query,
      CancellationToken cancellationToken = default)
      where TResponse : IResponse
    {
        return GetAsyncInternal<IReadOnlyList<TResponse>>(null, query, cancellationToken);
    }

    public Task<Result<IReadOnlyList<TResponse>>> GetManyAsync<TResponse>(
        IQueryRequest<TResponse> query,
        string? route,
        CancellationToken cancellationToken = default)
        where TResponse : IResponse
    {
        return GetAsyncInternal<IReadOnlyList<TResponse>>(route, query, cancellationToken);
    }

    #endregion

    #region Commands

    // POST
    public Task<Result<TResponse>> PostAsync<TResponse>(
             ICommandRequest<TResponse> command,
             CancellationToken cancellationToken = default)
             where TResponse : IResponse
    {
        return SendAsync(HttpMethod.Post, null, command, cancellationToken);
    }

    public Task<Result<TResponse>> PostAsync<TResponse>(
        ICommandRequest<TResponse> command,
        string? route,
        CancellationToken cancellationToken = default)
        where TResponse : IResponse
    {
        return SendAsync(HttpMethod.Post, route, command, cancellationToken);
    }

    // PUT 
    public Task<Result<TResponse>> PutAsync<TResponse>(
        ICommandRequest<TResponse> command,
        CancellationToken cancellationToken = default)
        where TResponse : IResponse
    {
        return SendAsync(HttpMethod.Put, null, command, cancellationToken);
    }

    public Task<Result<TResponse>> PutAsync<TResponse>(
        ICommandRequest<TResponse> command,
        string? route,
        CancellationToken cancellationToken = default)
        where TResponse : IResponse
    {
        return SendAsync(HttpMethod.Put, route, command, cancellationToken);
    }

    // DELETE 
    public Task<Result<TResponse>> DeleteAsync<TResponse>(
        ICommandRequest<TResponse> command,
        CancellationToken cancellationToken = default)
        where TResponse : IResponse
    {
        return SendAsync(HttpMethod.Delete, null, command, cancellationToken);
    }

    public Task<Result<TResponse>> DeleteAsync<TResponse>(
        ICommandRequest<TResponse> command,
        string? route,
        CancellationToken cancellationToken = default)
        where TResponse : IResponse
    {
        return SendAsync(HttpMethod.Delete, route, command, cancellationToken);
    }

    #endregion

    #region Validation

    public Task<bool> IsValidAsync(
       string[] parameterNames,
       object[] parameterValues,
       CancellationToken cancellationToken = default,
       [CallerMemberName] string callerMethodName = "")
    {
        var route = RouteHelper.CreateValidationRoute(callerMethodName);
        return IsValidAsync(parameterNames, parameterValues, route, cancellationToken);
    }

    private async Task<bool> IsValidAsync(
        string[] parameterNames,
        object[] parameterValues,
        string route,
        CancellationToken cancellationToken = default)
    {
        Guard.NotNull(parameterNames);
        Guard.NotNull(parameterValues);

        var boundRoute = RouteBinder.BindRoute(parameterNames, parameterValues, route);
        var keyValuePairs = HttpClientHelper.CreateFormKeyValuePairs(parameterNames, parameterValues);
        var requestUri = BuildUri(boundRoute);

        var cacheKey = CacheValidationHelper.CreateCacheKey(route, keyValuePairs);

        if (_validationCache.TryGetValue(cacheKey, out var isValid))
        {
            return isValid;
        }

        var messageFactory = new FormMessageFactory(HttpMethod.Post, requestUri, keyValuePairs);
        var result = await _executor.SendAsync<bool>(messageFactory, cancellationToken);
        if (result.IsSuccess)
        {
            _validationCache.Add(cacheKey, result.Value);
            return result.Value;
        }

        if (result.Error.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
#if DEBUG
            throw new BadRequestException(result.Error);
#endif
        }
        return false;
    }

    #endregion

    private Task<Result<TResponse>> GetAsyncInternal<TResponse>(
        string? route,
        IQueryRequest query,
        CancellationToken cancellationToken)
        where TResponse : notnull
    {
        var boundRoute = RouteBinder.BindRoute(query, route);
        var queryUri = HttpClientHelper.CreateQueryString(query);
        var requestUri = BuildUri(boundRoute, queryUri);
        var messageFactory = new NoContentMessageFactory(HttpMethod.Get, requestUri);
        return _executor.SendAsync<TResponse>(messageFactory, cancellationToken);
    }

    private Task<Result<TResponse>> SendAsync<TResponse>(
        HttpMethod method,
        string? routeTemplate,
        ICommandRequest<TResponse> command,
        CancellationToken cancellationToken)
        where TResponse : IResponse
    {
        var route = RouteBinder.BindRoute(command, routeTemplate);
        var requestUri = BuildUri(route);

        var messageFactory = new JsonMessageFactory(method, requestUri, command);
        return _executor.SendAsync<TResponse>(messageFactory, cancellationToken);
    }

    private Uri BuildUri(string? route, string? query = null)
    {
        var baseAddress = _executor.BaseAddress;
        var path = RouteHelper.Combine(_baseRoute, route);
        var uriBuilder = new UriBuilder(baseAddress)
        {
            Path = path,
            Query = query
        };
        return uriBuilder.Uri;
    }
}

