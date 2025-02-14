namespace AtendeLogo.Application.Exceptions;

public class RequestHandlerNotFoundException : Exception
{
    public RequestHandlerNotFoundException( 
        string message,
        params object[] args)
        : base(string.Format(message, args))
    {
    }
}
