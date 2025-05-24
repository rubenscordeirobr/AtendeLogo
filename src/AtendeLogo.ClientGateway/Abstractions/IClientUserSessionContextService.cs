
namespace AtendeLogo.ClientGateway.Abstractions;

public interface IClientUserSessionContextService
{
    Task InitializeAsync();
 
    Task ClearSessionContextAsync();
}
