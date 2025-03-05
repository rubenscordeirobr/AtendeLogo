using AtendeLogo.Application.Commands;
using AtendeLogo.Application.Contracts.Security;

namespace AtendeLogo.UseCases.Identities.Tenants.Commands;

public class CreateTenantCommandHandler
    : CommandHandler<CreateTenantCommand, CreateTenantResponse>
{
    public IIdentityUnitOfWork _unitOfWork;
    private ISecureConfiguration _secureConfiguration;

    public CreateTenantCommandHandler(
        IIdentityUnitOfWork unitOfWork,
        ISecureConfiguration secureConfiguration )
    {
        _unitOfWork = unitOfWork;
        _secureConfiguration = secureConfiguration;
    }

    protected override async Task<Result<CreateTenantResponse>> HandleAsync(
        CreateTenantCommand command, 
        CancellationToken cancellationToken)
    {

        var salt = _secureConfiguration.GetPasswordSalt();
        var password = Password.Create(command.Password, salt);

        if (!password.IsSuccess)
        {
            return Result.Failure<CreateTenantResponse>(password.Error);
        }

        var tenant = new Tenant(
            name: command.TenantName,
            fiscalCode: command.FiscalCode,
            email: command.Email,
            businessType: command.BusinessType,
            currency: command.Currency,
            country: command.Country,
            language: command.Language,
            tenantState: TenantState.New,
            tenantStatus: TenantStatus.Active,
            tenantType: command.TenantType,
            phoneNumber: command.PhoneNumber,
            timeZoneOffset: TimeZoneOffset.Default);
 
       var user = tenant.AddUser(
           name: command.Name,
           email: command.Email,
           userState: UserState.Active,
           userStatus: UserStatus.Online,
           tenantUserRole: TenantUserRole.Owner,
           phoneNumber: command.PhoneNumber,
           password.Value);

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            _unitOfWork.Add(tenant);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
           
            tenant.SetCreateOwner(user);
            _unitOfWork.Update(tenant);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            var response = new CreateTenantResponse(tenant.Id);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync(ex, cancellationToken);

            var error = new InternalError(ex,
                "CreateTenantCommandHandler.HandleAsync",
                "An error occurred while creating a tenant");

            return Result.Failure<CreateTenantResponse>(error);
        }
    }
}
