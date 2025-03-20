using AtendeLogo.Common.Utils;

namespace AtendeLogo.UseCases.Shared;
public static partial class DefaultValidationsExtensions
{
    public static IRuleBuilderOptions<T, PhoneNumber> PhoneNumber<T>(
        this IRuleBuilder<T, PhoneNumber> ruleBuilder,
        IJsonStringLocalizer<ValidationMessages> localizer)
    {
        return ruleBuilder
            .SetValidator(new PhoneNumberValidator(localizer));
    }

    public static IRuleBuilderOptions<T, string> PhoneNumber<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        IJsonStringLocalizer<ValidationMessages> localizer)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage(localizer["PhoneNumber.Number", "Phone number cannot be empty."])
            .MaximumLength(ValidationConstants.PhoneNumberMaxLength)
            .WithMessage(localizer["PhoneNumber.TooLong", "Phone number cannot be longer than {MaxLength} characters."])
            .Must(ValidationUtils.IsFullPhoneNumberValid);
    }
}

public class PhoneNumberValidator : AbstractValidator<PhoneNumber>
{
    public PhoneNumberValidator(IJsonStringLocalizer<ValidationMessages> localizer)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Number)
            .NotEmpty()
            .WithMessage(localizer["PhoneNumber.Number", "Phone number cannot be empty."])
            .MaximumLength(ValidationConstants.PhoneNumberMaxLength)
            .WithMessage(localizer["PhoneNumber.TooLong", "Phone number cannot be longer than {MaxLength} characters."])
            .Must(ValidationUtils.IsFullPhoneNumberValid);
    }
}
