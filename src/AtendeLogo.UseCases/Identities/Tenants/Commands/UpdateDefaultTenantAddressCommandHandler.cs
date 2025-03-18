using AtendeLogo.Application.Commands;
using AtendeLogo.UseCases.Common;

namespace AtendeLogo.UseCases.Identities.Tenants.Commands;

public sealed class UpdateDefaultTenantAddressCommandHandler
    : CommandHandler<UpdateDefaultTenantAddressCommand, SuccessResponse>
{
    public IIdentityUnitOfWork _unitOfWork;

    public UpdateDefaultTenantAddressCommandHandler(
        IIdentityUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    protected override async Task<Result<SuccessResponse>> HandleAsync(
        UpdateDefaultTenantAddressCommand command, CancellationToken cancellationToken)
    {
        var tenant = await _unitOfWork.Tenants
            .GetByIdAsync(command.Tenant_Id, cancellationToken, t => t.DefaultAddress);

        if (tenant == null)
        {
            return Result.NotFoundFailure<SuccessResponse>(
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
            return Result.Success(new SuccessResponse());
        }
        return Result.Failure<SuccessResponse>(result.Error);
    }
}

