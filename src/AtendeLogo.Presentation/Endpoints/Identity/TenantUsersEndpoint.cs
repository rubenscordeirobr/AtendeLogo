using AtendeLogo.Application.Contracts.Mediators;
using AtendeLogo.Presentation.Common;
using AtendeLogo.UseCases.Contracts.Identities;
using AtendeLogo.UseCases.Identities.Users.TenantUsers.Queries;

namespace AtendeLogo.Presentation.Endpoints.Identity;

[EndPoint("api/tenant-users")]
public class TenantUsersEndpoint : ApiEndpointBase, ITenantUserService
{
    private readonly IRequestMediator _mediator;

    public TenantUsersEndpoint(IRequestMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public Task<Result<TenantUserResponse>> GetTenantUserByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _mediator.GetSingleAsync(new GetTenantUserByIdQuery(id), cancellationToken);
    }

    [HttpGet("/", "email={email}")]
    public Task<Result<TenantUserResponse>> GetTenantUserByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return _mediator.GetSingleAsync(new GetTenantUserByEmailQuery(email), cancellationToken);
    }

    [HttpGet("/", "phone-number={phoneNumber}")]
    public Task<Result<TenantUserResponse>> GetTenantUserByPhoneNumberAsync(
        string phoneNumber,
        CancellationToken cancellationToken = default)
    {
        return _mediator.GetSingleAsync(new GetTenantUserByPhoneNumberQuery(phoneNumber), cancellationToken);
    }
      
    [HttpGet("/", "emailOrPhoneNumber={emailOrPhoneNumber}")]
    public Task<Result<TenantUserResponse>> GetTenantUserByEmailOrPhoneNumberAsync(
       string emailOrPhoneNumber,
       CancellationToken cancellationToken = default)
    {
        return _mediator.GetSingleAsync(new GetTenantUserByEmailOrPhoneNumberQuery(emailOrPhoneNumber), cancellationToken);
    }
}

