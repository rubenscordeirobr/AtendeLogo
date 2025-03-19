using AtendeLogo.Common.Utils;

namespace AtendeLogo.UseCases.Shared;
public static partial class DefaultValidationsExtensions
{
    public static IRuleBuilderOptions<T, FiscalCode> FiscalCodeCountry<T>(
        this IRuleBuilder<T, FiscalCode> ruleBuilder,
        IJsonStringLocalizer<ValidationMessages> localizer,
        Country country)
    {
        return ruleBuilder
            .SetValidator(new FiscalCodeValidator(country, localizer));

    }
    public static IRuleBuilderOptions<T, FiscalCode> FiscalCode<T>(
        this IRuleBuilder<T, FiscalCode> ruleBuilder,
        Func<T, Country> funcCountry,
        IJsonStringLocalizer<ValidationMessages> localizer)
    {
        return ruleBuilder
            .SetValidator((instance, fiscalCode) => new FiscalCodeValidator(funcCountry(instance), localizer));
    }
}
 
public class FiscalCodeValidator : AbstractValidator<FiscalCode>
{
    private readonly Country _country;
    private readonly IJsonStringLocalizer<ValidationMessages> _localizer;


    public FiscalCodeValidator(
        Country country,
        IJsonStringLocalizer<ValidationMessages> localizer)
    {
        _country = country;
        _localizer = localizer;

        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Value)
            .NotEmpty()
            .WithMessage(_localizer["FiscalCode.Value", "Fiscal code is required."])
            .MaximumLength(ValidationConstants.FiscalCodeMaxLength)
            .WithMessage(_localizer["FiscalCode.MaxLength", "Fiscal code cannot be longer than {MaxLength} characters."])
            .Must((fiscalCode, value) => FiscalCodeValidationUtils.IsValid(value, _country))
            .WithMessage(_localizer["FiscalCode.Invalid", "Invalid fiscal code."]);
    }
}
//public class FiscalCodeValidator<T> : PropertyValidator<T, FiscalCode>
//{
//    private readonly Country? _country;
//    private readonly Func<T, Country>? _funcCountry;

//    public override string Name
//        => "FiscarCodeValidator";

//    public FiscalCodeValidator(Country country)
//    {
//        _country = country;
//    }

//    public FiscalCodeValidator(Func<T, Country> funcCountry)
//    {
//        _funcCountry = funcCountry;
//    }

//    public override bool IsValid(ValidationContext<T> context, FiscalCode fiscalCode)
//    {
//        var country = GetCountry(context.InstanceToValidate);
//        return FiscalCodeValidationUtils.IsValid(fiscalCode.Value, country);
//    }

//    private Country GetCountry(T instanceToValidate)
//    {
//        if (_funcCountry != null)
//            return _funcCountry(instanceToValidate);

//        return _country ?? Country.Unknown;
//    }
//}
