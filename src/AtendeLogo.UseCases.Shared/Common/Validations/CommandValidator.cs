using FluentValidation.Results;
namespace AtendeLogo.UseCases.Common.Validations;

public abstract class CommandValidator<TCommand> : AbstractValidator<TCommand>
    where TCommand : ICommandRequest
{
    protected IJsonStringLocalizer<ValidationMessages> Localizer { get; }

    protected CommandValidator(IJsonStringLocalizer<ValidationMessages> localizer)
    {
        Localizer = localizer;
        ClassLevelCascadeMode = CascadeMode.Stop;
    }

    public override ValidationResult Validate(ValidationContext<TCommand> context)
    {
        Guard.NotNull(context);

        var result = base.Validate(context);
        return NormalizeContext(context, result);
    }

    public override async Task<ValidationResult> ValidateAsync(
        ValidationContext<TCommand> context,
        CancellationToken cancellation = default)
    {
        Guard.NotNull(context);

        var result = await base.ValidateAsync(context, cancellation);
        return NormalizeContext(context, result);
    }

    private ValidationResult NormalizeContext(
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
