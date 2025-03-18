namespace AtendeLogo.UseCases.Identities.Authentications.Commands;

public class TenantUserLoginCommandValidator : CommandValidator<TenantUserLoginCommand>
{
    private readonly ITenantAuthenticationValidationService _tenantValidationService;

    public TenantUserLoginCommandValidator(
        ITenantAuthenticationValidationService tenantValidationService,
        IJsonStringLocalizer<ValidationMessages> localizer)
        : base(localizer)
    {
        _tenantValidationService = tenantValidationService;

        RuleFor(x => x.EmailOrPhoneNumber).
            NotEmpty()
                .WithMessage(localizer["TenantAuthentication.EmailOrPhoneNumberRequired", "Email or phone number is required."])
           .MaximumLength(ValidationConstants.EmailMaxLength)
                .WithMessage(localizer["TenantAuthentication.EmailOrPhoneNumberMaxLength", $"Email or phone number must be less than {ValidationConstants.EmailMaxLength} characters."])
           .EmailAddressOrPhoneNumber()
                .WithMessage(localizer["TenantAuthentication.InvalidEmailOrPhoneNumber", "Invalid email or phone number."]);

        RuleFor(x => x.Password)
            .NotEmpty()
                .WithMessage(localizer["TenantAuthentication.PasswordRequired", "Password is required."])
            .MinimumLength(ValidationConstants.PasswordMinLength)
                .WithMessage(localizer["PasswordTooShort", "Password must be at least {MinLength} characters long"])
            .MaximumLength(ValidationConstants.PasswordMaxLength)
                .WithMessage(localizer["PasswordTooLong", "Password cannot be longer than {MaxLength} characters"]);


        //Async
        RuleFor(x => x.EmailOrPhoneNumber)
            .MustAsync(IsEmailOrPhoneNumberExitsAsync)
                .WithMessage(localizer["TenantAuthentication.EmailOrPhoneNumberNotExists", "Email or phone number does not exist."]);

        RuleFor(command => command)
            .MustAsync(VerifyTenantUserCredentialsAsync)
                .WithMessage(localizer["TenantAuthentication.InvalidCredentials", "Invalid credentials."]);
    }

    private Task<bool> VerifyTenantUserCredentialsAsync(
        TenantUserLoginCommand command,
        CancellationToken cancellationToken)
    {
        return _tenantValidationService
            .VerifyTenantUserCredentialsAsync(
                command.EmailOrPhoneNumber,
                command.Password,
                cancellationToken);
    }

    private Task<bool> IsEmailOrPhoneNumberExitsAsync(
        string emailOrPhoneNumber,
        CancellationToken cancellationToken)
    {
        return _tenantValidationService.EmailOrPhoneNumberExitsAsync(
            emailOrPhoneNumber,
            cancellationToken);
    }
}

