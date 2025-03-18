namespace AtendeLogo.Domain.Exceptions;

public class UnauthorizedSecurityException : DomainException
{
    public UnauthorizedSecurityException(string message) 
        : base(message)
    {
    }
}

