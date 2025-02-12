namespace AtendeLogo.Domain.Exceptions;

public class DomainSecurityException : DomainException
{
    public DomainSecurityException(string message) 
        : base(message)
    {
    }
}
