using AtendeLogo.Application.Contracts.Security;
using AtendeLogo.Domain.Entities.Identities.Factories;
using AtendeLogo.UseCases.Mappers.Identities;

namespace AtendeLogo.UseCases.Identities.Authentications.Commands;

public class AdminUserLoginCommandHandler : CommandHandler<AdminUserLoginCommand, AdminUserLoginResponse>
{
    private readonly IIdentityUnitOfWork _unitOfWork;
    private readonly IHttpContextSessionAccessor _httpContextSessionAccessor;
    private readonly IUserSessionManager _userSessionManager;
    private readonly ISecureConfiguration _secureConfiguration;
    private readonly IAuthenticationAttemptLimiterService _authenticationLimiter;
    private readonly ILogger<AdminUserLoginCommandHandler> _logger;

    public AdminUserLoginCommandHandler(
        IIdentityUnitOfWork unitOfWork,
        ISecureConfiguration secureConfiguration,
        IUserSessionManager userSessionManager,
        IHttpContextSessionAccessor httpContextSessionAccessor,
        IAuthenticationAttemptLimiterService authenticationValidator,
        ILogger<AdminUserLoginCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _secureConfiguration = secureConfiguration;
        _userSessionManager = userSessionManager;
        _httpContextSessionAccessor = httpContextSessionAccessor;
        _authenticationLimiter = authenticationValidator;
        _logger = logger;
    }

    protected override async Task<Result<AdminUserLoginResponse>> HandleAsync(
        AdminUserLoginCommand command,
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

            return Result.Failure<AdminUserLoginResponse>(
                new TooManyRequestsError(
                    "AdminUser.MaxAuthenticationReached",
                    message));
        }

        var user = await _unitOfWork.AdminUsers
            .GetByEmailOrPhoneNumberAsync(command.EmailOrPhoneNumber, cancellationToken);

        if (user is null)
        {
            await _authenticationLimiter.IncrementFailedAttemptsAsync(headerInfo.IpAddress, cancellationToken);

            return Result.Failure<AdminUserLoginResponse>(new NotFoundError("AdminUser.NotFound", "AdminUser not found"));
        }

        var salt = _secureConfiguration.GetPasswordSalt();
        if (!PasswordHelper.VerifyPassword(command.Password, user.Password.HashValue, salt))
        {
            await _authenticationLimiter.IncrementFailedAttemptsAsync(headerInfo.IpAddress, cancellationToken);
            return Result.Failure<AdminUserLoginResponse>(
                new AuthenticationError("AdminUser.InvalidPassword", "Invalid password"));
        }

        if (cancellationToken.IsCancellationRequested)
        {
            return Result.Failure<AdminUserLoginResponse>(
                new OperationCanceledError(null,
                    "AdminUserLoginCommandHandler.HandleAsync",
                    "Operation was canceled."));
        } 

        var userSession = UserSessionFactory.Create(
            user: user,
            clientHeaderInfo: headerInfo,
            authenticationType: AuthenticationType.Credentials,
            keepSession: command.KeepSession,
            tenant_id: null);

        _unitOfWork.Add(userSession);

        var result = await _unitOfWork.SaveChangesAsync(silent: true, cancellationToken);

        if (result.IsFailure)
        {
            return Result.Failure<AdminUserLoginResponse>(
                new InternalServerError(result.Exception,
                    "AdminUserLoginCommandHandler.SaveChangesAsync",
                     "Failed to save user session."));
        }


        await _userSessionManager.SetSessionAsync(userSession, user);

        var authorizationToken = _httpContextSessionAccessor.AuthorizationToken;

        Guard.NotNullOrWhiteSpace(authorizationToken);

        var sessionResponse = UserSessionMapper.ToResponse(userSession);
        var userResponse = UserMapper.ToResponse(user);

        var response = new AdminUserLoginResponse
        {
            AuthorizationToken = authorizationToken,
            User = userResponse,
            UserSession = sessionResponse
        };

        return Result.Success(response);
    }
}

