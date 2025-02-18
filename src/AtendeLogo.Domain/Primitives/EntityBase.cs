namespace AtendeLogo.Domain.Primitives;
public abstract class EntityBase
{
    public Guid Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime LastUpdatedAt { get; protected set; }
    public Guid CreatedSession_Id { get; protected set; }
    public Guid LastUpdatedSession_Id { get; protected set; }

    public virtual void SetCreateSession(Guid session_Id)
    {
        if (Id != Guid.Empty && !Id.IsZeroPrefixedGuid())
        {
            throw new InvalidOperationException("Cannot set create date for existing entity");
        }

        CreatedAt = default;
        LastUpdatedAt = CreatedAt;
        CreatedSession_Id = session_Id;
        LastUpdatedSession_Id = session_Id;
    }

    public void SetUpdateSession(Guid sessionId)
    {
        LastUpdatedAt = DateTime.UtcNow;
        LastUpdatedSession_Id = sessionId;
    }

    public override string ToString()
    {
        if (Id == default)
        {
            return $"{GetType().Name}: Id={Id}";
        }
        return $"{GetType().Name}: Id={Id}, CreatedAt={CreatedAt}, LastUpdatedAt={LastUpdatedAt}";
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is not EntityBase other ||
            GetType() != obj.GetType())
        {
            return false;
        }

        if (Id == default || other.Id == default)
        {
            return false;
        }
        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return (GetType().ToString() + Id).GetHashCode();
    }
}
