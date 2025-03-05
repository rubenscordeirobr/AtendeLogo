using AtendeLogo.Application.Commands;

namespace AtendeLogo.UseCases.Identities.Tenants.Commands;

public class UpdateDefaultTenantAddressCommandHandler
    : CommandHandler<UpdateDefaultTenantAddressCommand, UpdateDefaultTenantResponse>
{
    public IIdentityUnitOfWork _unitOfWork;

    public UpdateDefaultTenantAddressCommandHandler(
        IIdentityUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    protected override async Task<Result<UpdateDefaultTenantResponse>> HandleAsync(
        UpdateDefaultTenantAddressCommand command, CancellationToken cancellationToken)
    {
        var tenant = await _unitOfWork.TenantRepository
            .GetWithAddressByIdAsync(command.Tenant_Id);

        if (tenant == null)
        {
            return Result.NotFoundFailure<UpdateDefaultTenantResponse>(
                 "Tenant.NotFound",
                 $"Tenant with id {command.Tenant_Id} not found.");
        }

        var address = command.Address;
        var newAddress = tenant.AddAddress(
             command.AddressName,
             address.Street,
             address.Number,
             address.Complement,
             address.Neighborhood,
             address.City,
             address.State,
             address.ZipCode,
             address.Country);

        tenant.SetDefaultAddress(newAddress);

        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (result.IsSuccess)
        {
            return Result.Success(new UpdateDefaultTenantResponse());
        }
        return Result.Failure<UpdateDefaultTenantResponse>(result.Error);
    }
}

