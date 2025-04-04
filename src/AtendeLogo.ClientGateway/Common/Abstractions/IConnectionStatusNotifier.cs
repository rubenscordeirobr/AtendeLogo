namespace AtendeLogo.ClientGateway.Common.Abstractions;

public interface IConnectionStatusNotifier
{
    Task NotifyConnectionFailureAsync();
    void OnConnectionLost();
    void OnConnectionRestored();
}
