namespace AtendeLogo.Common.Exceptions;

public class CriticalNotFoundException : Exception
{
    public CriticalNotFoundException(string message)
        : base(message)
    {
    }
}

