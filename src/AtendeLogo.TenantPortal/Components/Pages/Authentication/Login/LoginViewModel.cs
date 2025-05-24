using AtendeLogo.UseCases.Abstractions.Identities;
using AtendeLogo.UseCases.Identities.Authentications.Commands;

namespace AtendeLogo.TenantPortal.Components.Pages.Authentication.Login;

public class LoginViewModel : ViewModelBase
{
    private readonly ITenantUserAuthenticationService _authenticationService;
    private readonly ICultureProvider _cultureProvider;
    public string? EmailOrPhoneNumber { get; set; }
    public string? Password { get; set; }
    public bool IsPersistent { get; set; }

    public LoginViewModel(
        ILogger<LoginViewModel> logger,
        IBusyIndicatorService busyIndicatorService,
        ICultureProvider cultureProvider,
        ITenantUserAuthenticationService authenticationService)
        : base(busyIndicatorService, logger)
    {
        _cultureProvider = cultureProvider;
        _authenticationService = authenticationService;
    }

    public TenantUserLoginCommand CreateCommand()
    {
        var emailOrPhoneNumber = SanitizeEmailOrPhoneNumber(EmailOrPhoneNumber);
        return new TenantUserLoginCommand
        {
            EmailOrPhoneNumber = emailOrPhoneNumber!,
            Password = Password!,
            IsPersistent = IsPersistent
        };
    }

    private string SanitizeEmailOrPhoneNumber(string? emailOrPhoneNumber)
    {
        if (!ValidationUtils.IsEmail(emailOrPhoneNumber))
        {
            if (ValidationUtils.IsFullPhoneNumberValid(emailOrPhoneNumber))
            {
                return SanitizeUtils.SanitizePhoneNumber(emailOrPhoneNumber!);
            }

            var country = _cultureProvider.GetCountry();
            if (ValidationUtils.IsNationalNumberValid(country, emailOrPhoneNumber))
            { 
                return PhoneNumberUtils.GetFullPhoneNumber(country, emailOrPhoneNumber!);
            }
        }
        return SanitizeUtils.SanitizeEmail(emailOrPhoneNumber!);
    }

    public async Task<Result<TenantUserLoginResponse>> LoginAsync()
    {
        return await RunWithBusyIndicatorAsync(LoginAsyncInternal);
    }

    private async Task<Result<TenantUserLoginResponse>> LoginAsyncInternal()
    {
        var command = CreateCommand();
        return await _authenticationService.LoginAsync(command);
    }
}
