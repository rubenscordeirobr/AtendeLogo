
namespace AtendeLogo.UI.Abstractions;

public interface IBusyIndicatorService
{
    
    public event Func<Task>? OnBusyAsync;
    public event Func<Task>? OnIdleAsync;

    void Busy();
    void Release();

 

    Task<Result<T>> RunWithBusyIndicatorAsync<T>(
        Func<Task<Result<T>>> operation,
        CancellationToken cancellationToken = default)
        where T : notnull;

}
