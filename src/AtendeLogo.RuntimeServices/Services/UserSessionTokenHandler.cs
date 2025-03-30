using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AtendeLogo.Application.Contracts.Security;
using AtendeLogo.Shared.Configuration;
using AtendeLogo.Shared.Factories;
using AtendeLogo.Shared.Models.Security;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AtendeLogo.RuntimeServices.Services;

public class UserSessionTokenHandler : IUserSessionTokenHandler
{
    private readonly JwtSecurityTokenHandler _tokenHandler = new();
    private readonly TokenValidationParameters _validationParameters;
    private readonly SigningCredentials _credentials;

    private readonly ILogger<UserSessionTokenHandler> _logger;

    public UserSessionTokenHandler(
        ISecureConfiguration secureConfiguration,
        ILogger<UserSessionTokenHandler> logger)
    {
        Guard.NotNull(secureConfiguration);

        _logger = logger;

        var symmetricSecurityKey = new SymmetricSecurityKey(
            SHA256.HashData(Encoding.UTF8.GetBytes(secureConfiguration.GetAuthenticationKey()))
        );

        _credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        _validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = secureConfiguration.GetJwtIssuer(),
            ValidAudience = secureConfiguration.GetJwtAudience(),
            IssuerSigningKey = symmetricSecurityKey,
            ClockSkew = TimeSpan.FromMinutes(2)
        };
    }

    public string WriteToken(UserSessionClaims userSessionClaims, bool keepSession)
    {
        Guard.NotNull(userSessionClaims);


        var expirationTime = UserSessionConfig.GetSessionExpiration(keepSession);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, userSessionClaims.Name),
            new Claim(ClaimTypes.Email, userSessionClaims.Email),
            new Claim(ClaimTypes.MobilePhone, userSessionClaims.PhoneNumber),
            new Claim(UserSessionClaimTypes.SessionId, userSessionClaims.Session_Id.ToString()),
            new Claim(ClaimTypes.Role, userSessionClaims.UserRole.ToString()),
            new Claim(UserSessionClaimTypes.UserType, userSessionClaims.UserType.ToString()),

        };

        var token = new JwtSecurityToken(
            issuer: _validationParameters.ValidIssuer,
            audience: _validationParameters.ValidAudience,
            claims: claims,
            expires: DateTime.UtcNow.Add(expirationTime),
            signingCredentials: _credentials
        );

        return _tokenHandler.WriteToken(token);
    }

    public UserSessionClaims? ReadToken(string token)
    {
        var principal = ValidateAndReadPrincipal(token, out var validatedToken);
        if (principal is null || validatedToken is not JwtSecurityToken jwt)
        {
            return null;
        }

        var name = principal.FindFirst(ClaimTypes.Name)?.Value;
        var email = principal.FindFirst(ClaimTypes.Email)?.Value;
        var phoneNumber = principal.FindFirst(ClaimTypes.MobilePhone)?.Value;
        var sessionId = principal.FindFirst(UserSessionClaimTypes.SessionId)?.Value;
        var userRole = principal.FindFirst(ClaimTypes.Role)?.Value;
        var userType = principal.FindFirst(UserSessionClaimTypes.UserType)?.Value;

        var result = UserSessionClaimsFactory.Create(
            name: name,
            email: email,
            phoneNumber: phoneNumber,
            sessionIdString: sessionId,
            userRoleString: userRole,
            userTypeString: userType,
            expiration: jwt.ValidTo);

        if (result.IsFailure)
        {
            _logger.LogError("Error reading user session token. {Token}. Code: {Code}, Message: {Message} ",
                token,
                result.Error.Code,
                result.Error.Message);
            return null;
        }
        return result.Value;
    }

    private ClaimsPrincipal? ValidateAndReadPrincipal(string authorizationToken, out SecurityToken? validatedToken)
    {
        validatedToken = null;
      
        if (!_tokenHandler.CanReadToken(authorizationToken))
        {
            _logger.LogError("JwtSecurityTokenHandler.CanReadToken. Invalid token: {AuthorizationToken}. ", authorizationToken);
            return null;
        }

        try
        {
            return _tokenHandler.ValidateToken(authorizationToken, _validationParameters, out validatedToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Invalid token: {AuthorizationToken}. Error : {Message}", authorizationToken, ex.Message);
            return null;
        }
    }
}
