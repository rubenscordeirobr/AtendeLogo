using AtendeLogo.Shared.Enums;

namespace AtendeLogo.Presentation.Common;

public abstract class ApiEndpointBase
{
    internal virtual async Task<ResponseResult> ProcessAsync(
        HttpMethodDescriptor descriptor, 
        object?[]? parameterValues)
    {
        var method = descriptor.Method;
        var methodResult = method.Invoke(
           this,
           parameterValues);
           
        var successStatusCode = descriptor.SuccessStatusCode;
        if (methodResult is not Task task)
        {
            return ResponseResult.SuccessWithStatus(successStatusCode, methodResult!);
        }

        await task.ConfigureAwait(false);

        var taskResult = task.GetResult();
        if (taskResult is not IResultValue resultValue)
        {
            return ResponseResult.SuccessWithStatus(successStatusCode, taskResult!);
        }

        if (resultValue.IsSuccess)
        {
            return ResponseResult.SuccessWithStatus(successStatusCode, resultValue.Value!);
        }
        return ResponseResult.Error(resultValue.Error);
    }

    #region IEndpointService
    public virtual ServiceRole ServiceRole
        => ServiceRole.General;
    public virtual string ServiceName
        => this.GetType().Name;

    public virtual bool IsAllowAnonymous
        => false;

    #endregion
}

