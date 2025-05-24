using System.Diagnostics;
using AtendeLogo.UseCases.Identities.Tenants.Commands;

//no sin
namespace AtendeLogo.TenantPortal.Components.Pages.Authentication.Create;

public class CreateTenantAccountViewModel : ViewModelBase
{
    private readonly ICultureProvider _cultureProvider;
    private readonly ITenantUserAuthenticationService _authenticationService;

    public string? FullName { get; set; }
    public string? BusinessName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
    public string? FiscalCode { get; set; }
    public bool IsPersistent { get; set; } = true;
    public BusinessType BusinessType { get; set; }
    public TenantType TenantType { get; set; }
    public PhoneNumber? PhoneNumber { get; set; }

    public CreateTenantAccountViewModel(
        ICultureProvider cultureProvider,
        ITenantUserAuthenticationService authenticationService,
        IBusyIndicatorService busyIndicatorService,
        ILogger<CreateTenantAccountViewModel> logger)
        : base(busyIndicatorService, logger)
    {
        _cultureProvider = cultureProvider;
        _authenticationService = authenticationService;

        if (Debugger.IsAttached)
        {
            FullName = "Teste Full Name";
            Email = "test@test.com.br";
            BusinessName = "BusinessName";
            Password = "Password10#";
            ConfirmPassword = "Password10#";
            PhoneNumber = new PhoneNumber("+5511912345678");
        }
    }

    public async Task<Result<CreateTenantAccountResponse>> CreateAccountAsync()
    {
        return await RunWithBusyIndicatorAsync(CreateAccountAsyncInternal);
    }

    private async Task<Result<CreateTenantAccountResponse>> CreateAccountAsyncInternal()
    {
        var command = CreateCommand();
        return await _authenticationService.CreateTenantAccountAsync(command);
    }

    public CreateTenantAccountCommand CreateCommand()
    {
        var email = SanitizeUtils.SanitizeEmail(Email);
        var fiscalCodeString = SanitizeUtils.SanitizeFiscalCode(FiscalCode);

        var country = _cultureProvider.GetCountry();
        var language = _cultureProvider.Language;
        var currency = _cultureProvider.GetCurrency();
        var tenantType = TenantTypeResolver.GetTenantType(country, fiscalCodeString);

        return new CreateTenantAccountCommand
        {
            Name = FullName!,
            BusinessName = BusinessName!,
            Email = email,
            PhoneNumber = PhoneNumber!,
            Password = Password!,
            IsPersistent = IsPersistent,
            BusinessType = BusinessType,
            FiscalCode = new FiscalCode(fiscalCodeString),
            Country = country,
            Language = language,
            Currency = currency,
            TenantType = tenantType
        };
    }
}
