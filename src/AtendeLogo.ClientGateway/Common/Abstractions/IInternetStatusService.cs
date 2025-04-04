namespace AtendeLogo.ClientGateway.Common.Abstractions;

public interface IInternetStatusService
{
    Task<bool> CheckInternetConnectionAsync();
    Task WaitForInternetConnectionAsync();
}
