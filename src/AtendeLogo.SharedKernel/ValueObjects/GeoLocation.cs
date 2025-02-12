namespace AtendeLogo.Shared.ValueObjects;

public sealed record GeoLocation : ValueObjectBase
{
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }

    public override string ToString()
       => $"Latitude: {Latitude}, Longitude: {Longitude}";

    public static Result<GeoLocation> Create(double latitude, double longitude)
    {
        if (latitude is < -90 or > 90)
        {
            return Result.ValidationFailure<GeoLocation>(
                "GeoLocation.InvalidLatitude",
                "Latitude must be between -90 and 90 degrees.");
        }

        if (longitude is < -180 or > 180)
        {
            return Result.ValidationFailure<GeoLocation>(
                "GeoLocation.InvalidLongitude",
                "Longitude must be between -180 and 180 degrees.");
        }

        return Result.Success(new GeoLocation
        {
            Latitude = latitude,
            Longitude = longitude
        });
    }

    public static GeoLocation Empty
        => new GeoLocation();
}

