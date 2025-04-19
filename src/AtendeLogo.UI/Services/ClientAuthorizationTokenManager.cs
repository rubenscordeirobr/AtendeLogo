using System.IdentityModel.Tokens.Jwt;
using AtendeLogo.ClientGateway.Common.Abstractions;
using AtendeLogo.Shared.Models.Security;
using AtendeLogo.UI.Extensions;
using Blazored.LocalStorage;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace AtendeLogo.UI.Services;

public class ClientAuthorizationTokenManager : IClientAuthorizationTokenManager
{
    private const string AuthorizationTokenKey = "AtendeLogo:AuthorizationToken";

    private readonly JwtSecurityTokenHandler _tokenHandler = new();
    private readonly IJSRuntime _jsRuntime;
    private readonly ILocalStorageService _localStorageService;
    private readonly ILogger<ClientAuthorizationTokenManager> _logger;

    private UserSessionClaims? _currentUserSessionClaims;

    public ClientAuthorizationTokenManager(
        IJSRuntime jsRuntime,
        ILocalStorageService localStorageService,
        ILogger<ClientAuthorizationTokenManager> logger)
    {
        _localStorageService = localStorageService;
        _logger = logger;
        _jsRuntime = jsRuntime;
    }

    public UserSessionClaims? GetUserSessionClaims()
    {
        return _currentUserSessionClaims;
    }

    public async Task<string?> GetAuthorizationTokenAsync()
    {
        if (_jsRuntime is null || !_jsRuntime.IsJsRuntimeInitialized())
        {
            return null;
        }
        return await _localStorageService.GetItemAsync<string>(AuthorizationTokenKey);
    }

    public async Task SetAuthorizationTokenAsync(string authorizationToken, bool keepSession)
    {
        Guard.NotNullOrWhiteSpace(authorizationToken);
   
        await _localStorageService.SetItemAsync(AuthorizationTokenKey, authorizationToken);
        UpdateUserSessionClaims(authorizationToken);
    }

    public async Task ValidateAuthorizationHeaderAsync(string? responseAuthorizationHeader)
    {
        if (_jsRuntime is null)
        {
            return;
        }

        var authorizationToken = JwtUtils.ExtractTokenFromAuthorizationHeader(responseAuthorizationHeader);
        if (authorizationToken is null)
        {
            await RemoveAuthorizationTokenAsync();
            return;
        }
        await SetAuthorizationTokenAsync(authorizationToken, true);
    }
     
    public async Task RemoveAuthorizationTokenAsync()
    {
        await _localStorageService.RemoveItemAsync(AuthorizationTokenKey);
        _currentUserSessionClaims = null;
    }

    private void UpdateUserSessionClaims(string token)
    {
        try
        {
            _currentUserSessionClaims = ReadToken(token);
        }
        catch (Exception ex)
        {
            _currentUserSessionClaims = null;
            _logger.LogError(ex, "Error reading client session token. {ClientToken}", token);
        }
    }

    private UserSessionClaims? ReadToken(string token)
    {
        var jwtToken = _tokenHandler.ReadJwtToken(token);
        var result = UserSessionClaimsFactory.Create(jwtToken.Claims, jwtToken.ValidTo);
        if (result.IsSuccess)
        {
            return result.Value;
        }

        _logger.LogError("Error reading user session token. {Token}. Code: {Code}, Message: {Message} ",
            token,
            result.Error.Code,
            result.Error.Message);

        return null;
    }
}
