using AtendeLogo.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AtendeLogo.Persistence.Common.Converters.InMemory;

internal class GeoLocationConverter : ValueConverter<GeoLocation, string>
{
    private GeoLocationConverter() : base(
        geoLocation => geoLocation.ToJson(),
        json => GeoLocation.Parse(json))
    {
    }

    internal static GeoLocationConverter Instance { get; } = new();
}
