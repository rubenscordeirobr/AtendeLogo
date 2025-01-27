namespace AtendeLogo.Domain.Entities.Activities;

public abstract class ActivityBase
{
    public string? Id { get; init; }
    public Guid? TenantId { get; init; }
    public Guid UserSessionId { get; init; }
    public DateTime ActivityDate { get; init; } 
    public string? Description { get; init; }
    public ActivityType ActivityType { get; protected set; }
}
