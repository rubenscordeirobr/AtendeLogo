namespace AtendeLogo.Domain.Domain.Interfaces;

public interface IEntityDeleted
{
    bool IsDeleted { get; set; }
    DateTime? DeletedAt { get; set; }
    Guid? DeletedSessionId { get; set; }
}