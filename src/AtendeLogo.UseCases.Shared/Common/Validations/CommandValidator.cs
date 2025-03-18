using FluentValidation.Results;
namespace AtendeLogo.UseCases.Common.Validations;

public abstract class CommandValidator<TCommand> : AbstractValidator<TCommand>
    where TCommand : ICommandRequest
{
    protected readonly IJsonStringLocalizer<ValidationMessages> Localized;

    protected CommandValidator(IJsonStringLocalizer<ValidationMessages> localizer)
    {
        Localized = localizer;
        ClassLevelCascadeMode = CascadeMode.Stop;
    }

    public override ValidationResult Validate(ValidationContext<TCommand> context)
    {
        var result = base.Validate(context);
        return NormalizarContext(context, result);
    }

    public override async Task<ValidationResult> ValidateAsync(
        ValidationContext<TCommand> context,
        CancellationToken cancellation = default)
    {
        var result = await base.ValidateAsync(context, cancellation);
        return NormalizarContext(context, result);
    }

    private ValidationResult NormalizarContext(
        ValidationContext<TCommand> context,
        ValidationResult result)
    {
        if (result.IsValid)
        {
            context.InstanceToValidate.ValidatedSuccessfullyAt = DateTime.UtcNow;
        }
        return result;
    }

}
