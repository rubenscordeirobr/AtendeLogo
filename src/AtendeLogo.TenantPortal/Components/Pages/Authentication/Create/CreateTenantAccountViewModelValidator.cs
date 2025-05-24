using AtendeLogo.UseCases.Identities.Tenants.Commands;

namespace AtendeLogo.TenantPortal.Components.Pages.Authentication.Create;

public class CreateTenantAccountViewModelValidator : AbstractValidator<CreateTenantAccountViewModel>
{
    public CreateTenantAccountViewModelValidator(IValidator<CreateTenantAccountCommand> commandValidator)
    {
        RuleFor(x => x.ConfirmPassword)
            .Must(IsConfirmPassword)
            .WithMessage("Password and confirmation password do not match.");

        RuleFor(x => x)
            .ValidateWithCommand(x => x.CreateCommand(), commandValidator);
    }

    private bool IsConfirmPassword(
        CreateTenantAccountViewModel viewModel, 
        string? confirmPassword)
    {
        if (string.IsNullOrWhiteSpace(viewModel.Password) )
        {
            return true;
        }

        if (!PasswordValidationUtils.IsCreatePasswordValid(viewModel.Password))
        {
            return true;
        }
        return viewModel.Password == confirmPassword;
        
    }

}

