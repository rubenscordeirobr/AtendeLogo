using System.Diagnostics;
using System.Security.Cryptography;
using AtendeLogo.Common;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;

namespace AtendeLogo.TenantPortal.BlazorServer.Services;

internal sealed class HybridBrowserStorageService : IStorageService
{
    private readonly ProtectedLocalStorage _localStorage;
    private readonly ProtectedSessionStorage _sessionStorage;
    private readonly IJSRuntime _jsRuntime;
    private readonly ILogger<HybridBrowserStorageService> _logger;

    public HybridBrowserStorageService(
        ProtectedLocalStorage localStorage,
        ProtectedSessionStorage sessionStorage,
        IJSRuntime jsRuntime,
        ILogger<HybridBrowserStorageService> logger)

    {
        _localStorage = localStorage;
        _sessionStorage = sessionStorage;
        _jsRuntime = jsRuntime;
        _logger = logger;
    }

    public async Task<string?> GetItemAsync(string storageKey)
    {
        return await GetItemAsync<string>(storageKey);
    }

    public async Task<T?> GetItemAsync<T>(string storageKey)
    {
        Guard.NotEmpty(storageKey);

        if (!ValidateJsRuntimeInitialization())
        {
            return default;
        }

        return await TryGetItemAsync<T>(storageKey);
    }

    public async Task SetItemAsync(string storageKey, string textValue, bool isPersistent)
    {
        await SetItemAsync<string>(storageKey, textValue, isPersistent);
    }

    public async Task SetItemAsync<T>(string storageKey, T value, bool isPersistent)
    {
        Guard.NotEmpty(storageKey);

        if (!ValidateJsRuntimeInitialization())
        {
            return;
        }

        await RemoveItemAsync(storageKey);

        if (value is null)
        {
            return;
        }

        await TrySetItemAsync<T>(storageKey, value, isPersistent);
    }

    public async Task RemoveItemAsync(string storageKey)
    {
        if (!ValidateJsRuntimeInitialization())
        {
            return;
        }
        await TryRemoveItemAsync(storageKey);
    }

    private async Task<T?> TryGetItemAsync<T>(string storageKey)
    {
        try
        {
            return await GetItemAsyncInternal<T>(storageKey);
        }
        catch (CryptographicException ex)
        {
            await RemoveItemAsync(storageKey);
            _logger.LogError(ex, "Error decrypting item from storage: {StorageKey}", storageKey);
            return default;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting item from storage: {StorageKey}", storageKey);
            return default;
        }
    }

    private async Task<T?> GetItemAsyncInternal<T>(string storageKey)
    {

        var localStorageResult = await _localStorage.GetAsync<T>(storageKey);
        if (localStorageResult.Success)
        {
            return localStorageResult.Value;
        }
        var sessionStorageResult = await _sessionStorage.GetAsync<T>(storageKey);
        if (sessionStorageResult.Success)
        {
            return sessionStorageResult.Value;
        }
        return default;
    }

    private async Task TrySetItemAsync<T>(string storageKey, T value, bool isPersistent)
    {
        try
        {
            await SetItemAsyncInternal(storageKey, value, isPersistent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting item in storage: {StorageKey}", storageKey);
        }
    }

    private async Task SetItemAsyncInternal<T>(string storageKey, T value, bool isPersistent)
    {
        if (isPersistent)
        {
            await _localStorage.SetAsync(storageKey, value!);
        }
        else
        {
            await _sessionStorage.SetAsync(storageKey, value!);
        }
    }

    private async Task TryRemoveItemAsync(string storageKey)
    {
        try
        {
            await _localStorage.DeleteAsync(storageKey);
            await _sessionStorage.DeleteAsync(storageKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing item from storage: {StorageKey}", storageKey);
        }
    }

    private bool ValidateJsRuntimeInitialization()
    {
        if (!_jsRuntime.IsJsRuntimeInitialized())
        {
            Debugger.Break();

            _logger.LogError("JavaScript runtime is not initialized. Session context cannot be stored. " +
                             "Ensure this method is called after the component has rendered, preferably in OnAfterRenderAsync.");
            return false;
        }
        return true;
    }
}

