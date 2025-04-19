
namespace AtendeLogo.UI.Abstractions;

public interface IBusyIndicatorService
{
    
    public event Func<Task>? OnBusyAsync;
    public event Func<Task>? OnIdleAsync;

    void Busy();
    void Release();

}
