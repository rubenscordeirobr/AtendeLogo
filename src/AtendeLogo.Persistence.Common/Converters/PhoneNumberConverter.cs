using AtendeLogo.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AtendeLogo.Persistence.Common.Converters;

internal class PhoneNumberConverter : ValueConverter<PhoneNumber, string>
{
    private PhoneNumberConverter() : base(
        phoneNumber => phoneNumber.Number,
        numberString => new PhoneNumber(numberString) )
    {
    }

    internal static PhoneNumberConverter Instance { get; } = new();
}
