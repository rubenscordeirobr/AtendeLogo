using AtendeLogo.Application.Commands;
using AtendeLogo.Application.Contracts.Security;
using AtendeLogo.Common.Helpers;
using AtendeLogo.Domain.Entities.Identities.Factories;

namespace AtendeLogo.UseCases.Identities.Authentications.Commands;

public class TenantUserLoginCommandHandler : CommandHandler<TenantUserLoginCommand, TenantUserLoginResponse>
{
    private readonly IIdentityUnitOfWork _unitOfWork;
    private readonly IUserSessionAccessor _userSessionAccessor;
    private readonly ISecureConfiguration _secureConfiguration;
    private readonly IAuthenticationAttemptLimiterService _authenticationLimiter;
    private readonly ISessionCacheService _sessionCacheService;
    private readonly ILogger<TenantUserLoginCommandHandler> _logger;

    public TenantUserLoginCommandHandler(
        IIdentityUnitOfWork unitOfWork,
        ISecureConfiguration secureConfiguration,
        IUserSessionAccessor userSessionAccessor,
        ISessionCacheService sessionCacheService,
        IAuthenticationAttemptLimiterService authenticationValidator,
        ILogger<TenantUserLoginCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _secureConfiguration = secureConfiguration;
        _userSessionAccessor = userSessionAccessor;
        _authenticationLimiter = authenticationValidator;
        _sessionCacheService = sessionCacheService;
        _logger = logger;
    }

    protected override async Task<Result<TenantUserLoginResponse>> HandleAsync(
        TenantUserLoginCommand command,
        CancellationToken cancellationToken)
    {
        var clientHeaderInfo = _userSessionAccessor.GetClientRequestHeaderInfo();
        var maxAuthenticationResult = await _authenticationLimiter.MaxAuthenticationReachedAsync(clientHeaderInfo.IpAddress);
        if (maxAuthenticationResult.IsMaxReached)
        {
            var totalMinutes = maxAuthenticationResult.ExpirationTime?.TotalMinutes ?? 0;
            var message = $"Too many failed login attempts. Please try again in {totalMinutes:0.0} minute(s).";

            _logger.LogWarning("The user with IP address {IpAddress} has reached the maximum number of authentication attempts.", clientHeaderInfo.IpAddress);

            return Result.Failure<TenantUserLoginResponse>(
                new ValidationError(
                    "TenantUser.MaxAuthenticationReached",
                    message));
        }

        var user = await _unitOfWork.TenantUsers
            .GetByEmailOrPhoneNumberAsync(command.EmailOrPhoneNumber, cancellationToken);

        if (user is null)
        {
            await _authenticationLimiter.IncrementFailedAttemptsAsync(clientHeaderInfo.IpAddress);

            return Result.Failure<TenantUserLoginResponse>(new NotFoundError("TenantUser.NotFound", "Tenant user not found"));
        }

        var salt = _secureConfiguration.GetPasswordSalt();
        if (!PasswordHelper.VerifyPassword(command.Password, user.Password.HashValue, salt))
        {
            await _authenticationLimiter.IncrementFailedAttemptsAsync(clientHeaderInfo.IpAddress);
            return Result.Failure<TenantUserLoginResponse>(new ValidationError("TenantUser.InvalidPassword", "Invalid password"));
        }

        if (cancellationToken.IsCancellationRequested)
        {

            return Result.Failure<TenantUserLoginResponse>(
                new OperationCanceledError(null, 
                    "TenantUserLoginCommandHandler.HandleAsync", 
                    "Operation was cancelled."));
        }

        var userSession = UserSessionFactory.Create(
            user: user,
            clientHeaderInfo: clientHeaderInfo,
            authenticationType: AuthenticationType.Credentials,
            rememberMe: command.RememberMe,
            tenant_id: user.Tenant_Id);

        _unitOfWork.Add(userSession);

        _userSessionAccessor.AddClientSessionCookie(userSession.ClientSessionToken);

        await _sessionCacheService.AddSessionAsync(userSession);

        var response = new TenantUserLoginResponse
        {
            ClientSessionToken = userSession.ClientSessionToken,
            User_Id = user.Id,
            Tenant_Id = user.Tenant_Id,
            Role = user.Role,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber.Number,
            FullName = user.Name,
            ProfilePictureUrl = user.ProfilePictureUrl,
            IsEmailVerified = user.EmailVerificationState == VerificationState.Verified,
            IsPhoneNumberVerified = user.PhoneNumberVerificationState == VerificationState.Verified
        };

        return Result.Success(response);
    }
}

