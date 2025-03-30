
using Microsoft.AspNetCore.Authorization;

namespace AtendeLogo.Presentation.Endpoints.Identity;

[AllowAnonymous]
[ServiceRole(ServiceRole.Authentication)]
[EndPoint(IdentityRouteConstants.TenantUserValidation)]
public class TenantUserValidationEndpoint : ApiEndpointBase, ITenantUserValidationService
{
    private readonly ITenantUserValidationService _validationService;
     
    public TenantUserValidationEndpoint(ITenantUserValidationService validationService)
    {
        _validationService = validationService;
    }

    [HttpFormValidation]
    public Task<bool> IsEmailUniqueAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return _validationService.IsEmailUniqueAsync(email, cancellationToken);
    }

    [HttpFormValidation]
    public Task<bool> IsEmailUniqueAsync(
        Guid currentUser_Id,
        string email,
        CancellationToken cancellationToken = default)
    {
        return _validationService.IsEmailUniqueAsync(currentUser_Id, email, cancellationToken);
    }

    [HttpFormValidation]
    public Task<bool> IsPhoneNumberUniqueAsync(
        string phoneNumber,
        CancellationToken cancellationToken = default)
    {
        return _validationService.IsPhoneNumberUniqueAsync(phoneNumber, cancellationToken);
    }

    [HttpFormValidation]
    public Task<bool> IsPhoneNumberUniqueAsync(
        Guid currentUser_Id,
        string number,
        CancellationToken cancellationToken = default)
    {
        return _validationService.IsPhoneNumberUniqueAsync(currentUser_Id, number, cancellationToken);
    }

    #region IEndpointService
 
  
    #endregion
}
