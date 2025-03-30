namespace AtendeLogo.ClientGateway.Common.Contracts;

public interface IInternetStatusService
{
    Task<bool> CheckInternetConnectionAsync();
    Task WaitForInternetConnectionAsync();
}
