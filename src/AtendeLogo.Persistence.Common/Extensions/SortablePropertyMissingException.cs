﻿namespace AtendeLogo.Persistence.Common.Extensions;

public class SortablePropertyMissingException : Exception
{
    public SortablePropertyMissingException(string message)
        : base(message)
    {
    }

}

