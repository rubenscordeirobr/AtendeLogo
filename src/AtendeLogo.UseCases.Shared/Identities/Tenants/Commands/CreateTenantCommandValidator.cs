namespace AtendeLogo.UseCases.Identities.Tenants.Commands;

public sealed class CreateTenantCommandValidator : CommandValidator<CreateTenantCommand>
{
    private readonly ITenantValidationService _tenantValidationService;

    public CreateTenantCommandValidator(
        ITenantValidationService tenantValidationService,
        IJsonStringLocalizer<ValidationMessages> localizer)
        : base(localizer)
    {
        Guard.NotNull(tenantValidationService);
        Guard.NotNull(localizer);

        _tenantValidationService = tenantValidationService;

        RuleFor(x=> x.Name)
            .NotEmpty()
            .WithMessage(localizer["Tenant.NameRequired", "Name is required."])
            .MinimumLength(ValidationConstants.NameMinLength)
            .WithMessage(localizer["Tenant.NameTooShort", "Name cannot be shorter than {MinLength} characters."])
            .MaximumLength(ValidationConstants.NameMaxLength)
            .WithMessage(localizer["Tenant.NameTooLong", "Name cannot be longer than {MaxLength} characters."])  
            .FullName()
            .WithMessage(localizer["Tenant.InvalidFullname", "Name must contain first name and last name."]);

        RuleFor(x => x.TenantName)
            .NotEmpty()
            .WithMessage(localizer["Tenant.TenantNameRequired", "Tenant name is required."])
            .MinimumLength(ValidationConstants.NameMinLength)
            .WithMessage(localizer["Tenant.TenantNameTooShort", "Tenant name cannot be shorter than {MinLength} characters."])
            .MaximumLength(ValidationConstants.NameMaxLength)
            .WithMessage(localizer["Tenant.TenantNameTooLong", "Tenant name cannot be longer than {MaxLength} characters."]);
         
        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(ValidationConstants.EmailMaxLength)
            .EmailAddressValid()
            .WithMessage(localizer["Tenant.InvalidEmail", "Invalid email address."]);
 
        RuleFor(x => x.PhoneNumber)
            .PhoneNumber(localizer);

        RuleFor(x => x.Password)
            .CreatePassword(localizer);

        RuleFor(x => x.Country)
            .IsInEnumValue()
            .WithMessage(localizer["Tenant.InvalidCountry", "Invalid country."]);

        RuleFor(x => x.Language)
            .NotEmpty()
            .WithMessage(localizer["Tenant.LanguageRequired", "Language is required."])
            .IsInEnumValue()
            .WithMessage(localizer["Tenant.InvalidLanguage", "Invalid language."]);

        RuleFor(x => x.Currency)
            .IsInEnumValue()
            .WithMessage(localizer["Tenant.InvalidCurrency", "Invalid currency."]);

        RuleFor(x => x.BusinessType)
            .IsInEnumValue()
            .WithMessage(localizer["Tenant.InvalidBusinessType", "Invalid business type."]);

        RuleFor(x => x.TenantType)
            .IsInEnumValue()
            .WithMessage(localizer["Tenant.InvalidTenantType", "Invalid tenant type."]);

        RuleFor(x => x.TenantType)
            .IsInEnumValue()
            .WithMessage(localizer["Tenant.InvalidTenantType", "Invalid tenant type."]);
         
        RuleFor(x => x.FiscalCode)
            .FiscalCode(x => x.Country, localizer)
            .WithMessage(localizer["Tenant.InvalidFiscalCode", "Invalid fiscal code."]);

        //Async validation
        RuleFor(x => x.PhoneNumber)
            .MustAsync(IsPhoneNumberAsync)
            .WithErrorCode("Tenant.PhoneNumberUniqueValidation")
            .WithMessage(
                localizer["Tenant.PhoneNumberUniqueValidation", "The phone number '{PropertyValue} is already in use."]);

        RuleFor(x => x.Email)
            .MustAsync(IsEmailUniqueAsync)
            .WithErrorCode("Tenant.EmailUniqueValidation")
            .WithMessage(
                localizer["Tenant.EmailUniqueValidation", "The e-mail '{PropertyValue} is already in use."]);

        RuleFor(x => x.FiscalCode)
            .MustAsync(IsFiscalCodeUniqueAsync)
            .WithErrorCode("Tenant.FiscalCodeUniqueValidation")
            .WithMessage(
                localizer["Tenant.FiscalCodeUniqueValidation",
                          "The fiscal code '{PropertyValue}' is already in use."]);
    }

    private async Task<bool> IsPhoneNumberAsync(PhoneNumber phoneNumber, CancellationToken token)
    {
        return await _tenantValidationService.IsPhoneNumberUniqueAsync(phoneNumber.Number, token);
    }

    private async Task<bool> IsEmailUniqueAsync(string email, CancellationToken token)
    {
        return await _tenantValidationService.IsEmailUniqueAsync(email, token);
    }

    private async Task<bool> IsFiscalCodeUniqueAsync(FiscalCode fiscalCode, CancellationToken token)
    {
        return await _tenantValidationService.IsFiscalCodeUniqueAsync(fiscalCode.Value, token);
    }
}
