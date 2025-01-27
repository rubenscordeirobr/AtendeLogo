namespace AtendeLogo.Domain.ValueObjects;

public sealed record GeoLocation(double Latitude, double Longitude)
{
    public static Result<GeoLocation> Create(double latitude, double longitude)
    {
        if (latitude is < -90 or > 90)
        {
            return Result.Failure<GeoLocation>(
                "GeoLocation.InvalidLatitude",
                "Latitude must be between -90 and 90 degrees.");
        }

        if (longitude is < -180 or > 180)
        {
            return Result.Failure<GeoLocation>(
                "GeoLocation.InvalidLongitude",
                "Longitude must be between -180 and 180 degrees.");
        }

        return Result.Success(new GeoLocation(latitude, longitude));
    }

    public override string ToString() => $"Latitude: {Latitude}, Longitude: {Longitude}";
}
