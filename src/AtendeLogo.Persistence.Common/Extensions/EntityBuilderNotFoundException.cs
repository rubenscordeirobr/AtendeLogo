﻿namespace AtendeLogo.Persistence.Common.Extensions;

public class EntityBuilderNotFoundException : Exception
{
    public EntityBuilderNotFoundException(string message)
        : base(message)
    {
    }
}

