namespace AtendeLogo.UI.Components.Common;

public partial class BusyIndicatorProvider  
{
    private long _busyCount;
    public bool IsBusy 
        => Interlocked.Read(ref _busyCount) > 0;

    [Inject]
    private IBusyIndicatorService BusyIndicatorService { get; set; } = default!;
 
    protected override void OnInitialized()
    {
        base.OnInitialized();

        BusyIndicatorService.OnBusyAsync += BusyAsync;
        BusyIndicatorService.OnIdleAsync += IdleAsync;
    }

    private async Task IdleAsync()
    {
        Interlocked.Decrement(ref _busyCount);
        StateHasChanged();
        await Task.CompletedTask;
    }

    private Task BusyAsync()
    {
        Interlocked.Increment(ref _busyCount);
        StateHasChanged();
        return Task.CompletedTask;
    }
   
}

