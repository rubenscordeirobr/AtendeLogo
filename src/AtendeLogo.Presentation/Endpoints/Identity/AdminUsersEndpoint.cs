using AtendeLogo.Application.Contracts.Mediators;
using AtendeLogo.Presentation.Common;
using AtendeLogo.UseCases.Contracts.Identities;
using AtendeLogo.UseCases.Identities.Users.AdminUsers.Queries;

namespace AtendeLogo.Presentation.Endpoints.Identity;

[EndPoint("api/admin-users")]
public class AdminUsersEndpoint : ApiEndpointBase, IAdminUserService
{
    private readonly IRequestMediator _mediator;
    public AdminUsersEndpoint(IRequestMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public Task<Result<AdminUserResponse>> GetAdminUserByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _mediator.GetSingleAsync(new GetAdminUserByIdQuery(id), cancellationToken);
    }

    [HttpGet("/", "email={email}")]
    public Task<Result<AdminUserResponse>> GetAdminUserByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return _mediator.GetSingleAsync(new GetAdminUserByEmailQuery(email), cancellationToken);
    }

    [HttpGet("/", "phone-number={phoneNumber}")]
    public Task<Result<AdminUserResponse>> GetAdminUserByPhoneNumberAsync(
        string phoneNumber,
        CancellationToken cancellationToken = default)
    {
        return _mediator.GetSingleAsync(new GetAdminUserByPhoneNumberQuery(phoneNumber), cancellationToken);
    }
  
    [HttpGet("/", "emailOrPhoneNumber={emailOrPhoneNumber}")]
    public Task<Result<AdminUserResponse>> GetAdminUserByEmailOrPhoneNumberAsync(
        string emailOrPhoneNumber, CancellationToken cancellationToken)
    {
        return _mediator.GetSingleAsync(new GetAdminUserByEmailOrPhoneNumberQuery(emailOrPhoneNumber), cancellationToken);
    }
}

