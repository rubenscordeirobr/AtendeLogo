namespace AtendeLogo.ClientGateway.Common.Contracts;

public interface IConnectionStatusNotifier
{
    Task NotifyConnectionFailureAsync();
    void OnConnectionLost();
    void OnConnectionRestored();
}
