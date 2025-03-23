namespace AtendeLogo.Common.Exceptions;

public class UnauthorizedSecurityException : Exception
{
    public UnauthorizedSecurityException(string message) 
        : base(message)
    {
    }
}

