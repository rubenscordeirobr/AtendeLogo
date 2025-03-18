using System.Reflection;
using AtendeLogo.Common.Attributes;
using FluentValidation.Validators;

namespace AtendeLogo.UseCases.Common.Validations;

public static partial class DefaultValidationsExtensions
{
    [Obsolete("Use IsInEnumValue instead.")]
    public static IRuleBuilderOptions<T, TProperty> IsInEnum<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
    {
        throw new Exception("Use IsInEnumValue instead.");
    }

    public static IRuleBuilderOptions<T, TProperty> IsInEnumValue<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
    {
        return ruleBuilder
            .SetValidator(new EnumValueValidator<T, TProperty>());

    }
}

public class EnumValueValidator<T, TProperty> : EnumValidator<T, TProperty>
{
    public override string Name
        => "EnumValueValidator";

    public override bool IsValid(ValidationContext<T> context, TProperty value)
    {
        if (!base.IsValid(context, value))
        {
            return false;
        }

        var member = EnumType.GetMember(value!.ToString()!)
            .FirstOrDefault();

        if (member is null)
            return false;

        return member.GetCustomAttribute<UndefinedValueAttribute>() == null;
    }
}
