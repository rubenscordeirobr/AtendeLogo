using AtendeLogo.UseCases.Identities.Authentications.Commands;
using FluentValidation;

namespace AtendeLogo.TenantPortal.Components.Pages.Authentication.Login;

public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
{
    public LoginViewModelValidator(IValidator<TenantUserLoginCommand> commandValidator)
    {
        RuleFor(x => x)
            .ValidateWithCommand(x => x.CreateCommand(), commandValidator);
    }
}
