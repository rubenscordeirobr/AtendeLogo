namespace AtendeLogo.Domain.Primitives.Contracts;

public interface ISoftDeletableEntity
{
    bool IsDeleted { get;   }
    DateTime? DeletedAt { get;   }
    Guid? DeletedSession_Id { get; }
}
