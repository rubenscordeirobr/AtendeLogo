﻿namespace AtendeLogo.Persistence.Common.Exceptions;

public class EnumTypeNotMappedException : EntityException
{
    public EnumTypeNotMappedException(string message) : base(message)
    {
    }
}

