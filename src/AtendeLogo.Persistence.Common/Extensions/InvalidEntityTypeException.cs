﻿namespace AtendeLogo.Persistence.Common.Extensions;

public class InvalidEntityTypeException : Exception
{
    public InvalidEntityTypeException(string message)
        : base(message)
    {
    }
}
