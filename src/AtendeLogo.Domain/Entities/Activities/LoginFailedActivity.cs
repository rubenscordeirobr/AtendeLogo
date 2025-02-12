namespace AtendeLogo.Domain.Entities.Activities;

public sealed record LoginFailedActivity : LoginActivity
{
    public required string? PasswordFailed { get; init; }
    public required string? UserAgent { get; init; }

    public sealed override ActivityType ActivityType
        => ActivityType.LoginFailed;

}
