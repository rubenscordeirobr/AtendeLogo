namespace AtendeLogo.Application.Exceptions;

public class CommandValidatorNotFoundException : Exception
{
    public CommandValidatorNotFoundException(
        string message, params object[] args) 
        : base(string.Format(message, args))
    {
    }
}
