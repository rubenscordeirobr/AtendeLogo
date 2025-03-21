namespace AtendeLogo.Common.Exceptions;

public class DeprecatedException : Exception
{
    public DeprecatedException(string message)
        : base(message)
    {
    }
}

