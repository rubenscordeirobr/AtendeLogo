namespace AtendeLogo.Shared.Configuration;

public static class UserSessionConfig
{
    public const int UpdateCheckIntervalMinutes = 5;

    private static readonly TimeSpan PersistentSessionExpiration = TimeSpan.FromDays(90);
    private static readonly TimeSpan DefaultSessionExpiration = TimeSpan.FromMinutes(30);

    private static readonly TimeSpan PersistentSessionRefreshThreshold = TimeSpan.FromDays(7);
    private static readonly TimeSpan DefaultSessionRefreshThreshold = TimeSpan.FromMinutes(5);

    public static TimeSpan GetSessionExpiration(bool keepSession)
    {
        return keepSession
            ? PersistentSessionExpiration
            : DefaultSessionExpiration;
    }
     
    public static bool NeedsRefreshSession(DateTime expiration, bool keepSession)
    {
        var refreshThreshold = GetRefreshThreshold(keepSession);
        return expiration.IsCloseTo(refreshThreshold);
    }

    private static TimeSpan GetRefreshThreshold(bool keepSession)
    {
        return keepSession
            ? PersistentSessionRefreshThreshold
            : DefaultSessionRefreshThreshold;
    }
}
