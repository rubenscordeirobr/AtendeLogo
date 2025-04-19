using AtendeLogo.Shared.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AtendeLogo.Shared.Localization;

public sealed class JsonStringLocalizerCache : IJsonStringLocalizerCache, IDisposable
{
    private readonly Dictionary<Culture, LocalizationResourceMap> _localizedStringsCache = new();
    private readonly SemaphoreSlim _cacheLock = new(1, 1);

    private readonly IServiceProvider _serviceProvider;
    private readonly JsonLocalizationCacheConfiguration _configuration;
    private readonly ILogger<JsonStringLocalizerCache> _logger;

    public JsonStringLocalizerCache(
        IServiceProvider serviceProvider,
        JsonLocalizationCacheConfiguration configuration,
        ILogger<JsonStringLocalizerCache> logger)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task LoadCultureAsync(Culture culture)
    {
        try
        {
            await _cacheLock.WaitAsync();

            culture = CultureHelper.Normalize(culture);

            if (_localizedStringsCache.ContainsKey(culture))
                return;

            await using var scope = _serviceProvider.CreateAsyncScope();
            var localizerService = scope.ServiceProvider.GetRequiredService<IJsonStringLocalizerService>();

            var result = await localizerService.GetLocalizationResourceMapAsync(culture);
            if (result.IsFailure)
            {
                throw new InvalidOperationException(
                    $"Failed to initialize localization cache for culture {culture}: {result.Error}");
            }
            _localizedStringsCache[culture] = result.Value;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Error initializing localization cache for culture {Culture}", culture);
            throw;
        }
        finally
        {
            _cacheLock.Release();
        }
    }

    public string GetLocalizedString(
        Culture culture,
        string resourceIdentifier,
        string localizationKey,
        string defaultValue)
    {
        Guard.NotNullOrWhiteSpace(resourceIdentifier);
        culture = CultureHelper.Normalize(culture);

        EnsureCultureCacheReady(culture);

        if (_localizedStringsCache.TryGetValue(culture, out var resourceMap))
        {
            if (resourceMap.TryGetValue(resourceIdentifier, out var localizationMap))
            {
                if (localizationMap.TryGetValue(localizationKey, out var localizedString))
                {
                    if (culture.IsDefaultCulture() && defaultValue != localizedString)
                    {
                        _ = UpdateDefaultLocalizedStringIfNeededAsync(resourceIdentifier, localizationKey, defaultValue);
                    }
                    return localizedString;
                }
            }
            _ = AddLocalizationStringIfNeededAsync(culture, resourceIdentifier, localizationKey, defaultValue);
        }
        return defaultValue;
    }
    private void EnsureCultureCacheReady(Culture culture)
    {
        if (_localizedStringsCache.ContainsKey(culture))
            return;

        throw new InvalidOperationException(
            $"Localization cache for culture {culture} is not initialized. " +
            $"Call InitializeAsync first.");
    }

    private async Task AddLocalizationStringIfNeededAsync(
        Culture culture,
        string resourceIdentifier,
        string localizationKey,
        string defaultValue)
    {
        if (_configuration.AutoAddMissingKeys)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var localizerService = scope.ServiceProvider.GetRequiredService<IJsonStringLocalizerService>();

            await localizerService.AddLocalizedStringAsync(
                culture,
                resourceIdentifier,
                localizationKey,
                defaultValue);
        }
    }

    private async Task UpdateDefaultLocalizedStringIfNeededAsync(
        string resourceKey,
        string localizationKey,
        string defaultValue)
    {
        if (_configuration.AutoUpdateDefaultKeys)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var localizerService = scope.ServiceProvider.GetRequiredService<IJsonStringLocalizerService>();

            await localizerService.UpdateDefaultLocalizedStringAsync(
                resourceKey,
                localizationKey,
                defaultValue);
        }
    }
     
    public void Dispose()
    {
        try
        {
            _cacheLock.Dispose();
        }
        catch
        {
            // Ignore exceptions during disposal
        }
        GC.SuppressFinalize(this);
    }
}
