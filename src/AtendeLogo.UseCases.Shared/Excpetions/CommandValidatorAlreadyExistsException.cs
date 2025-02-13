
namespace AtendeLogo.UseCases.Excpetions;

public class CommandValidatorAlreadyExistsException : Exception
{
    public CommandValidatorAlreadyExistsException(string message) : base(message)
    {
    }
}
