﻿namespace AtendeLogo.Persistence.Common.Exceptions;

public class MaxLengthNotDefinedException : EntityException
{
    public MaxLengthNotDefinedException(string message) : base(message)
    {
    }
}

