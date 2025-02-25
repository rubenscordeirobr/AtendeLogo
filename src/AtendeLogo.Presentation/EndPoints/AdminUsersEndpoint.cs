using AtendeLogo.Application.Contracts.Mediators;
using AtendeLogo.Presentation.Common;
using AtendeLogo.UseCases.Identities.Users.AdminUsers.Queries;
using AtendeLogo.UseCases.Identities.Users.AdminUsers.Services;

namespace AtendeLogo.Presentation.EndPoints;

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

    [HttpGet("email={email}")]
    public Task<Result<AdminUserResponse>> GetAdminUserByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return _mediator.GetSingleAsync(new GetAdminUserByEmailQuery(email), cancellationToken);
    }

    [HttpGet("phone-number={phoneNumber}")]
    public Task<Result<AdminUserResponse>> GetAdminUserByPhoneNumberAsync(
        string phoneNumber,
        CancellationToken cancellationToken = default)
    {
        return _mediator.GetSingleAsync(new GetAdminUserByPhoneNumberQuery(phoneNumber), cancellationToken);
    }

    [HttpGet("/", "email={email}")]
    public Task<Result<AdminUserResponse>> GetAdminUserByEmailQueryAsync(
      string email,
      CancellationToken cancellationToken = default)
    {
        return _mediator.GetSingleAsync(new GetAdminUserByEmailQuery(email), cancellationToken);
    }

    [HttpGet("/", "phone-number={phoneNumber}")]
    public Task<Result<AdminUserResponse>> GetAdminUserByPhoneNumberQueryAsync(
        string phoneNumber,
        CancellationToken cancellationToken = default)
    {
        return _mediator.GetSingleAsync(new GetAdminUserByPhoneNumberQuery(phoneNumber), cancellationToken);
    }

    [HttpGet("/", "emailOrPhoneNumber={emailOrPhoneNumber}")]
    public Task<Result<AdminUserResponse>> GetAdminUserByEmailOrPhoneNumberQueryAsync(
       string emailOrPhoneNumber,
       CancellationToken cancellationToken = default)
    {
        return _mediator.GetSingleAsync(new GetAdminUserByEmailOrPhoneNumberQuery(emailOrPhoneNumber), cancellationToken);
    }
}

