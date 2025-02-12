namespace AtendeLogo.Domain.Entities.Activities;

public abstract record LoginActivity : ActivityBase
{
    public required string IPAddress { get; init; }
}
