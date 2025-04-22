namespace AtendeLogo.UseCases.Identities.UserSessions.Commands;

public record class ChangeUserSessionLanguageCommand(
    Guid UserSession_Id,
    Language Language) 
    : CommandRequest<OperationResponse>;

public class ChangeUserSessionLanguageCommandValidator
    : CommandValidator<ChangeUserSessionLanguageCommand>
{
    public ChangeUserSessionLanguageCommandValidator(
        IJsonStringLocalizer<ChangeUserSessionLanguageCommand> localizer)
        : base(localizer)
    {
        Guard.NotNull(localizer);

        RuleFor(x => x.Language)
            .IsInEnumValue()
            .WithMessage(localizer["InvalidLanguage", "Invalid language."]);
    }
}
