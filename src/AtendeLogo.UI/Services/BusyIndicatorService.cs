
namespace AtendeLogo.UI.Services;
public class BusyIndicatorService : IBusyIndicatorService
{
    public event Func<Task>? OnBusyAsync;
    public event Func<Task>? OnIdleAsync;

    public void Busy()
    {
        OnBusyAsync?.Invoke();
    }

    public void Release()
    {
        OnIdleAsync?.Invoke();
    }
}
