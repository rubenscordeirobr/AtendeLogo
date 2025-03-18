
namespace AtendeLogo.UseCases.Identities.Tenants.Queries;

public sealed class GetTenantByIdQueryHandler : GetQueryResultHandler<GetTenantByIdQuery, TenantResponse>
{
    private readonly ITenantRepository _tenantRepository;

    public GetTenantByIdQueryHandler(
        ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public override async Task<Result<TenantResponse>> HandleAsync(
        GetTenantByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var tenant = await _tenantRepository.GetByIdAsync(query.Id,
            cancellationToken,
            t => t.DefaultAddress);

        if (tenant is null)
        {
            return Result.NotFoundFailure<TenantResponse>(
                "Tenant.NotFound",
                $"Tenant with id {query.Id} not found.");
        }

        var addressDto = AddressMapper.MapAddressToAddressDto(tenant.DefaultAddress);
        return Result.Success(new TenantResponse
        {
            Id = tenant.Id,
            Name = tenant.Name,
            Email = tenant.Email,
            PhoneNumber = tenant.PhoneNumber,
            Address = addressDto,
            TenantState = tenant.TenantState,
            TenantStatus = tenant.TenantStatus,
            BusinessType = tenant.BusinessType,
            Country = tenant.Country,
            Currency = tenant.Currency,
            FiscalCode = tenant.FiscalCode,
            Language = tenant.Language,
            TenantName = tenant.Name,
            TenantType = tenant.TenantType,
            TimeZoneOffset = tenant.TimeZoneOffset,
            DeletedSession_Id = tenant.DeletedSession_Id,
            IsDeleted = tenant.IsDeleted,
            CreatedAt = tenant.CreatedAt,
            DeletedAt = tenant.DeletedAt,
            LastUpdatedAt = tenant.LastUpdatedAt
        });

    }
}
