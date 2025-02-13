namespace AtendeLogo.Application.Exceptions;

public class CommandValidatorNotFoundException : Exception
{
    public CommandValidatorNotFoundException(
        string message) : base(message)
    {
    }
}
