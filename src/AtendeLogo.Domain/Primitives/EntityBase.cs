namespace AtendeLogo.Domain.Primitives;
public abstract class EntityBase
{
    public Guid Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime LastUpdatedAt { get; protected set; }
    public virtual Guid CreatedSession { get; protected set; }
    public Guid LastUpdatedSession { get; protected set; }

    public virtual void SetCreateDate(Guid sessionId)
    {
        if (Id != Guid.Empty)
        {
            throw new InvalidOperationException("Cannot set create date for existing entity");
        }

        CreatedAt = DateTime.UtcNow;
        LastUpdatedAt = CreatedAt;
        CreatedSession = sessionId;
        LastUpdatedSession = sessionId;
    }

    public void UpdateLastUpdateDate(Guid sessionId)
    {
        LastUpdatedAt = DateTime.UtcNow;
        LastUpdatedSession = sessionId;
    }

    public override string ToString()
    {
        return $"{GetType().Name}: Id={Id}, CreatedAt={CreatedAt}, LastUpdatedAt={LastUpdatedAt}";
    }
}

