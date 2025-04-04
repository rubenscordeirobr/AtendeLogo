namespace AtendeLogo.Application.Abstractions.Handlers;
public interface IApplicationHandler
{
    Task HandleAsync(object handlerObject);
}
