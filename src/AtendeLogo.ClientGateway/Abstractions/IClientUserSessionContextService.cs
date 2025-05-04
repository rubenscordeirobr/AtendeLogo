
namespace AtendeLogo.ClientGateway.Abstractions;

public interface IClientUserSessionContextService
{
    Task EnsureSessionContextAsync();
 
    Task ClearSessionContextAsync();
}
