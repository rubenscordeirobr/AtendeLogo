using System.Net;
using System.Reflection;
using AtendeLogo.Presentation.Common.Enums;
using AtendeLogo.Shared.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AtendeLogo.Presentation.Common;

public class HttpRequestHandler
{
    private readonly HttpContext _httpContext;
    private readonly Type _endpointType;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _looger;
    private readonly HttpMethodDescriptor _descriptor;

    private string EndpointName
        => _endpointType.Name;

    private CancellationToken CancellationToken
        => _httpContext.RequestAborted;

    private bool IsCancellationRequested
        => CancellationToken.IsCancellationRequested;

    public HttpRequestHandler(
        HttpContext _httpContext,
        Type endpointType,
        HttpMethodDescriptor descriptor)
    {
        this._httpContext = _httpContext;
        _endpointType = endpointType;
        _serviceProvider = _httpContext.RequestServices;
        _looger = _serviceProvider.GetRequiredService<ILogger<HttpRequestHandler>>();
        _descriptor = descriptor;
    }

    public async Task HandleAsync()
    {
        CancellationToken cancellationToken = _httpContext.RequestAborted;
        var responseResult = await GetResponseResultAsync();
        _httpContext.Response.StatusCode = responseResult.StatusCode;
        if (responseResult.IsSuccess)
        {
            await _httpContext.Response.WriteAsJsonAsync(responseResult.Response);
        }
        else
        {
            await _httpContext.Response.WriteAsJsonAsync(responseResult.ErroResult);
        }
    }

    public async Task<ResponseResult> GetResponseResultAsync()
    {
        var endpointInstance = GetEndPointServiceInstance();
        if (endpointInstance is null)
        {
            return ResponseResult.Error(
                HttpStatusCode.InternalServerError,
                "HttpRequestHandler.InvalidEndPointType",
                $"Type {EndpointName} is not found");
        }

        try
        {
            return await GetResponseAsync(endpointInstance);
        }
        catch (OperationCanceledException)
        {
            // Request was aborted; avoid logging as an error
            _looger.LogInformation($"Request to {EndpointName} was canceled by the client.");
            return ResponseResult.Error(ExtendedHttpStatusCode.RequestAborted, "HttpRequestHandler.RequestCancelled", "Request was canceled");
        }
        catch (Exception ex)
        {
            if (IsCancellationRequested)
            {
                _looger.LogInformation($"Request to {EndpointName} was canceled");
                return ResponseResult.Error(HttpStatusCode.BadRequest, "HttpRequestHandler.RequestCancelled", "Request was canceled");
            }

            _looger.LogError(ex, $"Error invoking method {_descriptor.Method.Name} on {EndpointName}");
            var errorCode = "HttpRequestHandler.ErrorInvokingMethod";
            return ResponseResult.Error(HttpStatusCode.InternalServerError, errorCode, ex.GetNestedMessage());
        }
    }

    private ApiEndpointBase? GetEndPointServiceInstance()
    {
        var endpointInstance = _serviceProvider.GetService(_endpointType);
        if (endpointInstance == null)
        {
            try
            {
                endpointInstance = ActivatorUtilities.CreateInstance(_serviceProvider, _endpointType);
            }
            catch (Exception ex)
            {
                _looger.LogError(ex, $"Error creating instance of {EndpointName}");
                return null;
            }
        }

        if (endpointInstance is null)
        {
            _looger.LogError($"Type {EndpointName} is not found");
            return null;
        }

        if (endpointInstance is not ApiEndpointBase endPoint)
        {
            _looger.LogError($"Type {EndpointName} is not an EndPointBase");
            return null;
        }
        return endPoint;
    }

    private async Task<ResponseResult> GetResponseAsync(ApiEndpointBase endpointInstance)
    {
        var parameterValuesResult = await GetParameterValuesAsync();
        if (parameterValuesResult.IsFailure)
        {
            return ResponseResult.Error(parameterValuesResult.Error);
        }

        if (CancellationToken.IsCancellationRequested)
        {
            return ResponseResult.Error(
                ExtendedHttpStatusCode.RequestAborted,
                "HttpRequestHandler.RequestCancelled",
                "Request was canceled");
        }

        var methodResult = _descriptor.Method.Invoke(
            endpointInstance, 
            parameterValuesResult.Value);

        if (methodResult is not Task task)
        {
            return GetSuccessResult(methodResult!);
        }

        await task.ConfigureAwait(false);

        var taskResult = task.GetResult();
        if (taskResult is not IResultValue resultValue)
        {
            return GetSuccessResult(taskResult!);
        }

        if (resultValue.IsSuccess)
        {
            return GetSuccessResult(resultValue.Value);
        }
        return ResponseResult.Error(resultValue.Error);
    }

    private ResponseResult GetSuccessResult(object response)
    {
        var successStatusCode = _descriptor.SuccessStatusCode;
        if (successStatusCode != HttpStatusCode.OK)
        {
            return ResponseResult.SuccessWithStatus(successStatusCode, response);
        }
        return ResponseResult.Ok(response);
    }

    private async Task<Result<object?[]>> GetParameterValuesAsync()
    {
        var parameters = _descriptor.Parameters;
        if (parameters.Length == 0)
        {
            return Result.Success(Array.Empty<object?>());
        }
         
        var parameterValuesResult = await GetParameterValuesAsync(parameters);
        if (parameterValuesResult.IsFailure)
        {
            return Result.Failure<object?[]>(parameterValuesResult.Error);
        }

        var parameterValues = parameterValuesResult.Value!;
        if (!_descriptor.HasCancellationToken)
        {
            return Result.Success(parameterValues);
        }

        var parameterValuesWithToken = new object?[parameters.Length + 1];
        for (var i = 0; i < parameters.Length; i++)
        {
            parameterValuesWithToken[i] = parameterValues[i];
        }
        parameterValuesWithToken[^1] = CancellationToken;
        return Result.Success(parameterValuesWithToken);
    }

    private async Task<Result<object?[]>> GetParameterValuesAsync(ParameterInfo[] parameters)
    {
        if (parameters.Length == 0)
        {
            return Result.Success(Array.Empty<object?>());
        }

        if (parameters.Length == 1)
        {
            var parameter = parameters[0];
            if (parameter.ParameterType.ImplementsGenericInterfaceDefinition(typeof(IRequest<>)))
            {
                var valueResult = await BodyParameterBinder.BindParameterAsync(
                    _descriptor,
                    _httpContext, 
                    parameter);

                if (valueResult.IsSuccess)
                {
                    return Result.Success(new object?[] { valueResult.Value });
                }
                return Result.Failure<object?[]>(valueResult.Error);
            }
        }

        var parameterValues = new object?[parameters.Length];
        for (var i = 0; i < parameters.Length; i++)
        {
            var parameter = parameters[i];
            var paramterValueResult = GetParameterValue(parameter);
            if (paramterValueResult.IsFailure)
            {
                return Result.Failure<object?[]>(paramterValueResult.Error);
            }
            parameterValues[i] = paramterValueResult.Value;
        }
        return Result.Success(parameterValues);
    }

    private Result<object> GetParameterValue(ParameterInfo parameter)
    {
        if (_descriptor.ParameterToQueryKeyMap.ContainsKey(parameter))
        {
            var queryKey = _descriptor.ParameterToQueryKeyMap[parameter];
            return QueryParameterBinder.BindParameter(
                _descriptor,
                _httpContext,
                parameter,
                queryKey);
        }

        if (_descriptor.RouteParameters.Contains(parameter))
        {
            return RouteParameterBinder.BindParameter(
                _descriptor,
                _httpContext,
                parameter);
        }

        if (parameter.HasDefaultValue)
        {
            return Result.Success(parameter.DefaultValue!);
        }

        return Result.Failure<object>(
            new BadRequestError(
                "ParameterNotFound",
                $"Error in method '{_descriptor.Method.Name}' of '{_descriptor.Method.DeclaringType?.Name}': " +
                $"Missing required parameter '{parameter.Name}'."));
    }
}
