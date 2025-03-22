using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text.Json;
using AtendeLogo.Common.Enums;
using AtendeLogo.Common.Mappers;
using AtendeLogo.Presentation.Common.Binders;
using AtendeLogo.Presentation.Constants;
using AtendeLogo.Shared.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AtendeLogo.Presentation.Common;

internal sealed class HttpRequestExecutor
{
    private readonly HttpContext _httpContext;
    private readonly Type _endpointType;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;
    private readonly HttpMethodDescriptor _descriptor;
    private readonly ApiEndpointBase? _endpointInstance;

    private string EndpointName
        => _endpointType.Name;

    private CancellationToken CancellationToken
        => _httpContext.RequestAborted;

    private bool IsCancellationRequested
        => CancellationToken.IsCancellationRequested;

    internal HttpRequestExecutor(
        HttpContext httpContext,
        Type endpointType,
        HttpMethodDescriptor descriptor)
    {
        _httpContext = httpContext;
        _endpointType = endpointType;
        _serviceProvider = httpContext.RequestServices;
        _logger = _serviceProvider.GetRequiredService<ILogger<HttpRequestExecutor>>();
        _descriptor = descriptor;
        _endpointInstance = GetEndPointServiceInstance();
    }

    public async Task HandleAsync()
    {
        var cancellationToken = _httpContext.RequestAborted;
        var responseResult = await GetResponseResultAsync();

        _httpContext.Response.StatusCode = responseResult.StatusCode;

        var jsonOptions = GetJsonSerializerOptions();
        var response = responseResult.IsSuccess
            ? responseResult.Response
            : responseResult.ErrorResponse;

        await WriteResponseAsync(response, jsonOptions, cancellationToken);
    }

    private async Task WriteResponseAsync(
        object? response,
        JsonSerializerOptions jsonOptions,
        CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested || response is null)
        {
            return;
        }
        try
        {
            await _httpContext.Response.WriteAsJsonAsync(response, response.GetType(), jsonOptions, cancellationToken);
        }
        catch (Exception ex)
        {
            Log(logLevel: LogLevel.Warning,
                "HttpRequestExecutor.WriteResponseAsync",
                ex.GetNestedMessage());
        }
    }

    public async Task<ResponseResult> GetResponseResultAsync()
    {
        if (_endpointInstance is null)
        {
            _httpContext.Items.Add(HttpContextItensConstants.EndpointInstance, _endpointInstance);

            return ResponseResult.Error(
                HttpStatusCode.InternalServerError,
                "HttpRequestExecutor.InvalidEndPointType",
                $"Type {EndpointName} is not found");
        }

        try
        {
            return await GetResponseAsync();
        }
        catch (OperationCanceledException ex)
        {
            Log(LogLevel.Information, "HttpRequestExecutor.RequestCancelled", ex.GetNestedMessage());
            return ResponseResult.Error(ExtendedHttpStatusCode.RequestAborted,
                "HttpRequestExecutor.RequestCancelled",
                "Request was canceled");
        }
        catch (Exception ex)
        {
            if (IsCancellationRequested)
            {
                Log(LogLevel.Information, "HttpRequestExecutor.RequestCancelled", ex.GetNestedMessage());
                return ResponseResult.Error(HttpStatusCode.BadRequest,
                    "HttpRequestExecutor.RequestCancelled",
                    "Request was canceled");
            }
            else
            {
                Log(LogLevel.Critical, "HttpRequestExecutor.RequestCancelled", ex.GetNestedMessage());
                var errorCode = "HttpRequestExecutor.ErrorInvokingMethod";
                return ResponseResult.Error(HttpStatusCode.InternalServerError, errorCode, ex.GetNestedMessage());
            }
        }
    }
    private ApiEndpointBase? GetEndPointServiceInstance()
    {
        var endpointInstance = CreateEndpointServiceInstance();
        if (endpointInstance is not ApiEndpointBase endPoint)
        {
            _logger.LogCritical("Type {EndpointName} is not an EndPointBase", EndpointName);
            return null;
        }
        return endPoint;
    }

    private object? CreateEndpointServiceInstance()
    {
        var endpointInstance = _serviceProvider.GetService(_endpointType);
        if (endpointInstance is not null)
        {
            return endpointInstance;
        }
        try
        {
            return ActivatorUtilities.CreateInstance(_serviceProvider, _endpointType);
        }
        catch (Exception ex)
        {
            var message = $"Error creating instance of {EndpointName}. {ex.GetNestedMessage()}";
            Log(LogLevel.Critical, "HttpRequestExecutor.ErrorCreatingInstance", message);
            return null;
        }
    }

    private async Task<ResponseResult> GetResponseAsync()
    {
        var parameterValuesResult = await GetParameterValuesAsync();
        if (parameterValuesResult.IsFailure)
        {
#if DEBUG
            Debugger.Break();
#endif
            return ResponseResult.Error(parameterValuesResult.Error);
        }

        if (CancellationToken.IsCancellationRequested)
        {
            return ResponseResult.Error(
                ExtendedHttpStatusCode.RequestAborted,
                "HttpRequestExecutor.RequestCancelled",
                "Request was canceled");
        }

        var methodResult = _descriptor.Method.Invoke(
            _endpointInstance,
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

        Log(resultValue.Error);

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
            var parameterValueResult = GetParameterValue(parameter);
            if (parameterValueResult.IsFailure)
            {
                return Result.Failure<object?[]>(parameterValueResult.Error);
            }
            parameterValues[i] = parameterValueResult.Value;
        }
        return Result.Success(parameterValues);
    }

    private Result<object> GetParameterValue(ParameterInfo parameter)
    {
        if (_descriptor.OperationParameterToKeyMap.ContainsKey(parameter))
        {
            var queryKey = _descriptor.OperationParameterToKeyMap[parameter];
            return OperationParameterBinder.BindParameter(
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

    private JsonSerializerOptions GetJsonSerializerOptions()
    {
        var options = _endpointInstance?.GetJsonSerializerOptions()
            ?? JsonSerializerOptions.Web;

        JsonUtils.EnableIndentationInDevelopment(options);
        return options;
    }

    private void Log(Error error)
    {
        var level = ErrorLogLevelMapper.MapErrorLevel(error);
        Log(level, error.Code, error.Message);
    }

    private void Log(
        LogLevel logLevel,
        string errorCode,
        string errorMessage)
    {
        Log(logLevel, null, errorCode, errorMessage);
    }

    private void Log(
        LogLevel logLevel,
        Exception? exception,
        string errorCode,
        string errorMessage)
    {

        Debugger.Break();

        var requestUri = _httpContext.Request.GetDisplayUrl();
        var methodName = _descriptor.Method.Name;
        var httpVerb = _descriptor.HttpVerb;

#if DEBUG
        if (logLevel != LogLevel.Information)
        {
            Debugger.Break();
        }
#endif

        _logger.Log(
             logLevel,
             exception,
            "HTTP request. URI: {Uri}, Verb: {HttpVerb}, Endpoint {EndpointName}, Method: {MethodName}, ErrorCode: {ErrorCode}, ErrorMessage: {ErrorMessage}",
             requestUri,
             httpVerb,
             EndpointName,
             methodName,
             errorCode,
             errorMessage);
    }
}
