namespace AtendeLogo.UseCases.Identities.Authentications.Commands;

public class TenantUserLogoutCommandValidator : CommandValidator<TenantUserLogoutCommand>
{
    public TenantUserLogoutCommandValidator(
        IJsonStringLocalizer<ValidationMessages> localizer)
        : base(localizer)
    {
        Guard.NotNull(localizer);

        RuleFor(x => x.ClientSessionToken)
            .NotEmpty()
                .WithMessage(localizer["TenantUserLogout.AuthTokenRequired", "Auth token is required."])
            .MaximumLength(ValidationConstants.AuthTokenMaxLength)
                .WithMessage(localizer["TenantUserLogout.AuthTokenMaxLength", "Auth token must have a maximum length of {MaxLength}."]);

    }
}
