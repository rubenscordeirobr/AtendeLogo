using AtendeLogo.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AtendeLogo.Persistence.Common.Converters;

internal class FiscalCodeConverter : ValueConverter<FiscalCode, string>
{
    private FiscalCodeConverter() : base(
        fiscalCode => fiscalCode.Value,
        value => new FiscalCode(value))
    {
    }

    internal static FiscalCodeConverter Instance { get; } = new();
}
