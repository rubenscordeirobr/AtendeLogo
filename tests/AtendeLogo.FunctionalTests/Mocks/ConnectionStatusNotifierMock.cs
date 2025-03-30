namespace AtendeLogo.FunctionalTests.Mocks;

public class ConnectionStatusNotifierMock : IConnectionStatusNotifier
{
    public Task NotifyConnectionFailureAsync()
    {
        return Task.CompletedTask;
    }

    public void OnConnectionLost()
    {
        // Do nothing
    }

    public void OnConnectionRestored()
    {
        // Do nothing

    }
}
