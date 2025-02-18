using AtendeLogo.Common.Utils;
using FluentValidation;
using FluentValidation.Validators;

namespace AtendeLogo.UseCases.Shared;
public static partial class DefaultValidationsExtensions
{
    public static IRuleBuilderOptions<T, string> FiscalCodeCountry<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        Country country)
    {
        return ruleBuilder
            .SetValidator(new FiscalCodeValidator<T>(country));

    }
    public static IRuleBuilderOptions<T, string> FiscalCode<T>(
        this IRuleBuilderOptions<T, string> ruleBuilder,
        Func<T, Country> funcCountry)
    {
        return ruleBuilder
            .SetValidator(new FiscalCodeValidator<T>(funcCountry));
    }
}

public class FiscalCodeValidator<T> : PropertyValidator<T, string>
{
    private readonly Country? _country;
    private readonly Func<T, Country>? _funcCountry;
  
    public override string Name
        => "FiscarCodeValidator";

    public FiscalCodeValidator(Country country)
    {
        _country = country;
    }

    public FiscalCodeValidator(Func<T, Country> funcCountry)
    {
        _funcCountry = funcCountry;
    }

    public override bool IsValid(ValidationContext<T> context, string value)
    {
        var coutry = GetCountry(context.InstanceToValidate);
        return FiscalCodeValidationUtils.IsValid(value, coutry);
    }

    private Country GetCountry(T instanceToValidate)
    {
        if (_funcCountry != null)
            return _funcCountry(instanceToValidate);

        return _country ?? Country.Unknown;
    }
}
