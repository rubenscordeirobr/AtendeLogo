﻿namespace AtendeLogo.Domain.Exceptions;

public class DomainEventException : DomainException
{
    public DomainEventException(string message)
        : base(message)
    {
    }
}
