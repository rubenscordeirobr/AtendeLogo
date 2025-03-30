namespace AtendeLogo.Domain.Primitives.Contracts;

public interface IEntityBase
{
    Guid Id { get; }
    DateTime CreatedAt { get; }
    DateTime LastUpdatedAt { get; }
    Guid CreatedSession_Id { get; }
    Guid LastUpdatedSession_Id { get; }
}
