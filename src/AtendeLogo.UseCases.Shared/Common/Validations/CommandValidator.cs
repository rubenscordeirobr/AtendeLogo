using FluentValidation;
namespace AtendeLogo.UseCases.Common.Validations;

public abstract class CommandValidator<TCommand> : AbstractValidator<TCommand>
{
    protected readonly IJsonStringLocalizer<ValidationMessages> Localized;

    protected CommandValidator(IJsonStringLocalizer<ValidationMessages> localizer)
    {
        Localized = localizer;
        ClassLevelCascadeMode = CascadeMode.Stop;
    }
}
