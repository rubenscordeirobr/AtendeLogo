using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Validators;

namespace AtendeLogo.UseCases.Shared;

public static partial class DefaultValidationsExtensions
{
    [Obsolete("Use EmailAddressValid ")]
    public static IRuleBuilderOptions<T, string> EmailAddress<T>(this IRuleBuilder<T, string> ruleBuilder, EmailValidationMode mode = EmailValidationMode.AspNetCoreCompatible)
    {
        throw new Exception($"Use {nameof(EmailAddressValid)}");
    }
    public static IRuleBuilderOptions<T, string> EmailAddressValid<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.SetValidator(new EmailValidValidator<T>());
    }
}

public class EmailValidValidator<T> : PropertyValidator<T, string>, IEmailValidator
{
    private static readonly Regex _emailRegex = new(
         @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-!#$%&'*+/=?^_`{|}~\w]|\.(?!\.))+)(?<=\S)@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$",
         RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture,
         TimeSpan.FromMilliseconds(500));

    public override string Name
        => "EmailValidValidator";
 
    public override bool IsValid(ValidationContext<T> context, string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return true;

        if (value.Length > ValidationConstants.EmailMaxLength)
            return false; //prevent long values from causing a stack overflow

        return _emailRegex.IsMatch(value);
    }
}
