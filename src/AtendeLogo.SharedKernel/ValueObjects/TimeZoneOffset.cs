namespace AtendeLogo.Shared.ValueObjects;

public sealed record TimeZoneOffset
{
    public string Offset { get; }
    public string Location { get; }
    public TimeSpan OffsetTimeSpan { get; }

    private TimeZoneOffset(TimeSpan offsetTimeSpan, string offset, string location)
    {
        OffsetTimeSpan = offsetTimeSpan;
        Offset = offset;
        Location = location;
    }

    public static Result<TimeZoneOffset> Create(string offset, string location)
    {
        if (string.IsNullOrWhiteSpace(offset))
        {
            return Result.ValidationFailure<TimeZoneOffset>(
                "TimeZone.OffsetEmpty",
                "Offset cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(location))
        {
            return Result.ValidationFailure<TimeZoneOffset>(
                "TimeZine.LocationEmpty",
                "Location cannot be empty.");
        }

        if (!TimeSpan.TryParse(offset, out var offsetTimeSpan))
        {
            return Result.ValidationFailure<TimeZoneOffset>(
                "TimeZone.InvalidOffset",
                $"Invalid offset. {offset}");
        }
        return Result.Success(new TimeZoneOffset(offsetTimeSpan, offset, location));
    }
}
