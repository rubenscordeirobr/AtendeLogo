namespace AtendeLogo.Domain.Entities.Activities;

public sealed record LoginSuccessfulActivity : LoginActivity
{
    public required AuthenticationType AuthenticationType { get; init; }

    public sealed override ActivityType ActivityType
        => ActivityType.LoginSuccessful;
}
