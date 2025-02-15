using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AtendeLogo.Persistence.Common.Converters;

public class NullableGuidValueConverter : ValueConverter<Guid?, Guid?>
{
    private NullableGuidValueConverter() : base(
        guid => Convert(guid),
        guid => Convert(guid))
    {
    }

    internal static Guid? Convert(Guid? value)
    {
        return value is null || value == default
            ? null
            : value;
    }

    public static NullableGuidValueConverter Instance { get; } = new();
}
