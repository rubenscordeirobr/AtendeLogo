﻿using System.IdentityModel.Tokens.Jwt;
using AtendeLogo.Shared.Factories;
using AtendeLogo.Shared.Models.Security;
using Microsoft.Extensions.Logging;

namespace AtendeLogo.FunctionalTests.Mocks;

public class ClientAuthorizationTokenManagerMock : IClientAuthorizationTokenManager
{
    private string? _authorizationToken = string.Empty;
    private readonly ILogger<ClientAuthorizationTokenManagerMock> _logger;

    private readonly JwtSecurityTokenHandler _tokenHandler = new();
 
    public ClientAuthorizationTokenManagerMock(ILogger<ClientAuthorizationTokenManagerMock> logger)
    {
        _logger = logger;

    }

    public Task SetAuthorizationTokenAsync(string authorizationToken, bool keepSession)
    {
        _authorizationToken = authorizationToken;
        return Task.CompletedTask;
    }

    public Task ValidateAuthorizationHeaderAsync(string? responseAuthorizationHeader)
    {
        var responseAuthorizationToken = JwtUtils.ExtractTokenFromAuthorizationHeader(responseAuthorizationHeader);
        if (_authorizationToken != responseAuthorizationToken)
        {
            _authorizationToken = responseAuthorizationToken;
            _logger.LogWarning("Authorization token was changed to {AuthorizationToken}", responseAuthorizationHeader);
        }
        return Task.CompletedTask;
    }

    public Task RemoveAuthorizationTokenAsync()
    {
        _authorizationToken = null;
        return Task.CompletedTask;
    }

    public Task<string?> GetAuthorizationTokenAsync()
    {
        return Task.FromResult(_authorizationToken);
    }

    public UserSessionClaims? GetUserSessionClaims()
    {
        var authorizationToken = _authorizationToken;
        if (_authorizationToken is null)
        {
            return null;
        }
         
        if (!_tokenHandler.CanReadToken(authorizationToken))
        {
            return null;
        }

        try
        {
            var jwt = _tokenHandler.ReadJwtToken(authorizationToken);
            var claims = jwt.Claims;
            var result = UserSessionClaimsFactory.Create(claims, jwt.ValidTo);

            if (result.IsFailure)
            {
                _logger.LogError("Error reading user session token. {Token}. Code: {Code}, Message: {Message} ",
                    authorizationToken,
                    result.Error.Code,
                    result.Error.Message);
                return null;
            }
            return result.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading user session token. {Token}", authorizationToken);
            return null;
        }
    }
}
