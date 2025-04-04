﻿namespace AtendeLogo.Domain.Primitives.Abstractions;

public interface ISoftDeletableEntity
{
    bool IsDeleted { get;   }
    DateTime? DeletedAt { get;   }
    Guid? DeletedSession_Id { get; }
}
