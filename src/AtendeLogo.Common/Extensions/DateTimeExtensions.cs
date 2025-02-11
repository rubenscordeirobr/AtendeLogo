namespace AtendeLogo.Common.Extensions;
public static class DateTimeExtensions
{
    public static bool IsExpired(
        this DateTime dateTime,
        TimeSpan expirationTime)
    {
        return dateTime.Add(expirationTime) < DateTime.UtcNow;
    }
}
