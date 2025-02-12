namespace AtendeLogo.Application.Contracts.Handlers;
public interface IApplicationHandler
{
    Task HandleAsync(object handlerObject);
}
