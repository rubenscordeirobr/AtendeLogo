using AtendeLogo.Presentation.Common;
using AtendeLogo.UseCases.Contracts.Identities;

namespace AtendeLogo.Presentation.Endpoints.Identity;

[EndPoint("api/tenant-validation")]
public class TenantValidationEndpoint : ApiEndpointBase, ITenantValidationService
{
    public override bool IsAllowAnonymous
        => true;

    private readonly ITenantValidationService _tenantValidationService;

    public TenantValidationEndpoint(ITenantValidationService tenantValidationService)
    {
        _tenantValidationService = tenantValidationService;
    }

    [HttpPost]
    public Task<bool> IsEmailUniqueAsync(string email, CancellationToken token)
    {
        return _tenantValidationService.IsEmailUniqueAsync(email, token);

    }

    [HttpPost]
    public Task<bool> IsEmailUniqueAsync(
        Guid currentTenant_Id,
        Guid currentTenantOwner_Id,
        string email,
        CancellationToken token)
    {
        return _tenantValidationService.IsEmailUniqueAsync(
            currentTenant_Id,
            currentTenantOwner_Id,
            email,
            token);
    }

    [HttpPost]
    public Task<bool> IsFiscalCodeUniqueAsync(string fiscalCode, CancellationToken token)
    {
        return _tenantValidationService.IsFiscalCodeUniqueAsync(fiscalCode, token);
    }

    [HttpPost]
    public Task<bool> IsFiscalCodeUniqueAsync(
        Guid currentTenant_Id,
        string fiscalCode,
        CancellationToken token)
    {
        return _tenantValidationService.IsFiscalCodeUniqueAsync(
            currentTenant_Id,
            fiscalCode, token);
    }
}

