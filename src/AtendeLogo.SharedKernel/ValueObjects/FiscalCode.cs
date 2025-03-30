using System.Text.Json.Serialization;
using AtendeLogo.Common.Utils;

namespace AtendeLogo.Shared.ValueObjects;

public sealed record FiscalCode : ValueObjectBase
{
    public string Value { get; }

    [JsonConstructor]
    public FiscalCode(string value)
    {
        Value = value.GetOnlyNumbers();
    }
     
    public static Result<FiscalCode> Create(string value, Country country)
    {
        if (!FiscalCodeValidationUtils.IsValid(value, country))
        {
            var validationError = new ValidationError("FiscalCode.Invalid", "Invalid fiscal code.");
            return Result.Failure<FiscalCode>(validationError);
        }
        return Result.Success(new FiscalCode(value));
    }
}

