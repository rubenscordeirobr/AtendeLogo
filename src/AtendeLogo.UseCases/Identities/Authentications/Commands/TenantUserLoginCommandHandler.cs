using AtendeLogo.Application.Contracts.Security;
using AtendeLogo.Application.Extensions;
using AtendeLogo.Domain.Entities.Identities.Factories;
using AtendeLogo.UseCases.Mappers.Identities;

namespace AtendeLogo.UseCases.Identities.Authentications.Commands;

public class TenantUserLoginCommandHandler : CommandHandler<TenantUserLoginCommand, TenantUserLoginResponse>
{
    private readonly IIdentityUnitOfWork _unitOfWork;
    private readonly IHttpContextSessionAccessor _httpContextSessionAccessor;
    private readonly IUserSessionManager _userSessionManager;
    private readonly ISecureConfiguration _secureConfiguration;
    private readonly IAuthenticationAttemptLimiterService _authenticationLimiter;
    private readonly ILogger<TenantUserLoginCommandHandler> _logger;

    public TenantUserLoginCommandHandler(
        IIdentityUnitOfWork unitOfWork,
        ISecureConfiguration secureConfiguration,
        IUserSessionManager userSessionManager,
        IHttpContextSessionAccessor httpContextSessionAccessor,
        IAuthenticationAttemptLimiterService authenticationValidator,
        ILogger<TenantUserLoginCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _secureConfiguration = secureConfiguration;
        _userSessionManager = userSessionManager;
        _httpContextSessionAccessor = httpContextSessionAccessor;
        _authenticationLimiter = authenticationValidator;
        _logger = logger;
    }

    protected override async Task<Result<TenantUserLoginResponse>> HandleAsync(
        TenantUserLoginCommand command,
        CancellationToken cancellationToken)
    {
        Guard.NotNull(command);

        var headerInfo = _httpContextSessionAccessor.RequestHeaderInfo;

        var maxAuthenticationResult = await _authenticationLimiter.MaxAuthenticationReachedAsync(
            headerInfo.IpAddress, cancellationToken);

        if (maxAuthenticationResult.IsMaxReached)
        {
            var totalMinutes = maxAuthenticationResult.ExpirationTime?.TotalMinutes ?? 0;
            var message = $"Too many failed login attempts. Please try again in {totalMinutes:0.0} minute(s).";

            _logger.LogWarning("The user with IP address {IpAddress} has reached the maximum number of authentication attempts.", headerInfo.IpAddress);

            return Result.Failure<TenantUserLoginResponse>(
                new TooManyRequestsError(
                    "TenantUser.MaxAuthenticationReached",
                    message));
        }

        var user = await _unitOfWork.TenantUsers
            .GetByEmailOrPhoneNumberAsync(command.EmailOrPhoneNumber, cancellationToken);

        if (user is null)
        {
            await _authenticationLimiter.IncrementFailedAttemptsAsync(headerInfo.IpAddress, cancellationToken);

            return Result.Failure<TenantUserLoginResponse>(new NotFoundError("TenantUser.NotFound", "Tenant user not found"));
        }

        var salt = _secureConfiguration.GetPasswordSalt();
        if (!PasswordHelper.VerifyPassword(command.Password, user.Password.HashValue, salt))
        {
            await _authenticationLimiter.IncrementFailedAttemptsAsync(headerInfo.IpAddress, cancellationToken);
            return Result.Failure<TenantUserLoginResponse>(
                new AuthenticationError("TenantUser.InvalidPassword", "Invalid password"));
        }

        if (cancellationToken.IsCancellationRequested)
        {
            return Result.Failure<TenantUserLoginResponse>(
                new OperationCanceledError(null,
                    "TenantUserLoginCommandHandler.HandleAsync",
                    "Operation was canceled."));
        }

        var tenant = await _unitOfWork.Tenants.NoTracking().GetByIdAsync(user.Tenant_Id, cancellationToken);
        if (tenant is null)
        {
            return Result.Failure<TenantUserLoginResponse>(
                new CriticalNotFoundError("TenantUser.TenantNotFound", $"Tenant Id {user.Tenant_Id} from TenantUser Id {user.Id} not found."));
        }

        if (!tenant.IsActive())
        {
            return Result.Failure<TenantUserLoginResponse>(
                new AuthenticationError("TenantUser.TenantInactive", "Tenant is inactive."));
        }

        var userSession = UserSessionFactory.Create(
            user: user,
            clientHeaderInfo: headerInfo,
            authenticationType: AuthenticationType.Credentials,
            keepSession: command.KeepSession,
            tenant_id: user.Tenant_Id);

        _unitOfWork.Add(userSession);

        var result = await _unitOfWork.SaveChangesAsync(silent: true, cancellationToken);

        if (result.IsFailure)
        {
            return Result.Failure<TenantUserLoginResponse>(
                new InternalServerError(result.Exception,
                "TenantUserLoginCommandHandler.SaveChangesAsync",
                "Failed to save user session."));
        }


        await _userSessionManager.SetSessionAsync(userSession, user);

        var authorizationToken = _httpContextSessionAccessor.AuthorizationToken;

        Guard.NotNullOrWhiteSpace(authorizationToken);

        var sessionResponse = UserSessionMapper.ToResponse(userSession);
        var userResponse = UserMapper.ToResponse(user);
        var tenantResponse = TenantMapper.ToResponse(tenant);

        var response = new TenantUserLoginResponse
        {
            AuthorizationToken = authorizationToken,
            User = userResponse,
            Tenant = tenantResponse,
            UserSession = sessionResponse
        };

        return Result.Success(response);
    }
}

