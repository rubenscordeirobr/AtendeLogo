
using Microsoft.Extensions.Logging;

namespace AtendeLogo.UI.Services;
public class BusyIndicatorService : IBusyIndicatorService
{
    public event Func<Task>? OnBusyAsync;
    public event Func<Task>? OnIdleAsync;

    private readonly ILogger _logger;

    public BusyIndicatorService(
        ILogger<BusyIndicatorService> logger)
    {
        _logger = logger;
    }

    public void Busy()
    {
        OnBusyAsync?.Invoke();
    }

    public void Release()
    {
        OnIdleAsync?.Invoke();
    }

    public async Task<Result<T>> RunWithBusyIndicatorAsync<T>(
        Func<Task<Result<T>>> operation,
        CancellationToken cancellationToken = default)
        where T : notnull
    {
        Guard.NotNull(operation);

        try
        {
            this.Busy();

            var result = await operation();

            return result;
        }
        catch (Exception ex)
        {
            var errorCode = this.GetErrorCode(nameof(RunWithBusyIndicatorAsync));
            var error = new UnknownError(ex, errorCode, ex.Message);
             
            _logger.LogError(ex,
                    "An error occurred while executing the operation. Error code: {ErrorCode}. Message: {Message}",
                    errorCode,
                    ex.GetNestedMessage());

            return Result.Failure<T>(error);
        }
        finally
        {
            this.Release();
        }
    }
}
