using AtendeLogo.Presentation.Common;
using AtendeLogo.Shared.Enums;
using AtendeLogo.UseCases.Contracts.Identities;

namespace AtendeLogo.Presentation.Endpoints.Identity;

[EndPoint("api/identity/tenant-authentication-validation")]
public class TenantAuthenticationValidationEndpoint : ApiEndpointBase, ITenantAuthenticationValidationService
{
    private readonly ITenantAuthenticationValidationService _tenantAuthenticationValidationService;

    public TenantAuthenticationValidationEndpoint(
        ITenantAuthenticationValidationService tenantAuthenticationValidationService)
    {
        _tenantAuthenticationValidationService = tenantAuthenticationValidationService;
    }

    [HttpPost]
    public Task<bool> VerifyTenantUserCredentialsAsync(
        string emailOrPhoneNumber,
        string password,
        CancellationToken cancellationToken)
    {
        return _tenantAuthenticationValidationService
            .VerifyTenantUserCredentialsAsync(emailOrPhoneNumber, password, cancellationToken);
    }

    [HttpPost]
    public Task<bool> EmailOrPhoneNumberExitsAsync(
        string emailOrPhoneNumber,
        CancellationToken cancellationToken)
    {
        return _tenantAuthenticationValidationService
            .EmailOrPhoneNumberExitsAsync(emailOrPhoneNumber, cancellationToken);
    }

    #region IEndpointService
    public override bool IsAllowAnonymous
        => true;
    public override ServiceRole ServiceRole
        => ServiceRole.UserAuthentication;
    #endregion
}

