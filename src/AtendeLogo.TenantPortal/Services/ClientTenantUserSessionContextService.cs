using AtendeLogo.ClientGateway.Abstractions;
using Microsoft.JSInterop;

namespace AtendeLogo.TenantPortal.Services;

public class ClientTenantUserSessionContextService : IClientTenantUserSessionContextService
{
    private const string StorageKey = "TenantUserSessionContext";

    private readonly IJSRuntime _jsRuntime;
    private readonly IStorageService _storageService;
    private readonly ILogger<ClientTenantUserSessionContextService> _logger;
    private TenantUserSessionContext? _currentSessionContext;

    public ClientTenantUserSessionContextService(
        IJSRuntime jsRuntime,
        IStorageService storageService,
        ILogger<ClientTenantUserSessionContextService> logger)
    {
        _jsRuntime = jsRuntime;
        _storageService = storageService;
        _logger = logger;
    }

    public TenantUserSessionContext? SessionContext
        => _currentSessionContext;

    public async Task<TenantUserSessionContext?> GetSessionContextAsync()
    {
        if(_currentSessionContext is not null)
        {
            return _currentSessionContext;
        }

        if (!_jsRuntime.IsJsRuntimeInitialized())
        {
            _logger.LogError("JS Runtime is not initialized. Session context will not be stored.");
            return null;
        }

        await InitializeAsync();
        return _currentSessionContext;
    }

    public async Task SetSessionContextAsync(
        TenantUserSessionContext sessionContext, 
        bool isPersistent)
    {
     
        if (_currentSessionContext != null && _currentSessionContext.Equals(sessionContext))
        {
            return;
        }
    
        if (!_jsRuntime.IsJsRuntimeInitialized())
        {
            _logger.LogError("JS Runtime is not initialized. Session context will not be stored.");
            return;
        }

        _currentSessionContext = sessionContext;
        await _storageService.SetItemAsync(StorageKey, sessionContext, isPersistent);
    }

    public async Task InitializeAsync()
    {
        if (_currentSessionContext != null || !_jsRuntime.IsJsRuntimeInitialized())
        {
            return;
        }
         
        var stored = await _storageService.GetItemAsync<TenantUserSessionContext>(StorageKey);
        _currentSessionContext = stored;
    }

    public async Task ClearSessionContextAsync()
    {
        if (!_jsRuntime.IsJsRuntimeInitialized())
        {
            _logger.LogError("JS Runtime is not initialized. Session context will not be stored.");
            return;
        }
        _currentSessionContext = null;
        await _storageService.RemoveItemAsync(StorageKey);
    }
}

