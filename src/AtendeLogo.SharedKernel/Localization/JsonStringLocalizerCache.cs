using AtendeLogo.Shared.Abstractions;
using AtendeLogo.Shared.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AtendeLogo.Shared.Localization;

public sealed class JsonStringLocalizerCache : IJsonStringLocalizerCache, IDisposable
{
    private readonly Dictionary<Language, LocalizationResourceMap> _localizedStringsCache = new();
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

    public async Task LoadLanguageAsync(Language language)
    {
        try
        {
            await _cacheLock.WaitAsync();

            language = LanguageHelper.Normalize(language);

            if (_localizedStringsCache.ContainsKey(language))
                return;

            await using var scope = _serviceProvider.CreateAsyncScope();
            var localizerService = scope.ServiceProvider.GetRequiredService<IJsonStringLocalizerService>();

            var result = await localizerService.GetLocalizationResourceMapAsync(language);
            if (result.IsFailure)
            {
                throw new InvalidOperationException(
                    $"Failed to initialize localization cache for language {language}: {result.Error}");
            }
            _localizedStringsCache[language] = result.Value;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Error initializing localization cache for language {Language}", language);
            throw;
        }
        finally
        {
            _cacheLock.Release();
        }
    }

    public string GetLocalizedString(
        Language language,
        string resourceIdentifier,
        string localizationKey,
        string defaultValue)
    {
        Guard.NotNullOrWhiteSpace(resourceIdentifier);
        language = LanguageHelper.Normalize(language);

        EnsureLanguageInitialized(language);

        if (_localizedStringsCache.TryGetValue(language, out var resourceMap))
        {
            if (resourceMap.TryGetValue(resourceIdentifier, out var localizationMap))
            {
                if (localizationMap.TryGetValue(localizationKey, out var localizedString))
                {
                    if (language.IsDefaultLanguage() && defaultValue != localizedString)
                    {
                        _ = UpdateDefaultLocalizedStringIfNeededAsync(resourceIdentifier, localizationKey, defaultValue);
                    }
                    return localizedString;
                }
            }
            _ = AddLocalizationStringIfNeededAsync(language, resourceIdentifier, localizationKey, defaultValue);
        }
        return defaultValue;
    }
    private void EnsureLanguageInitialized(Language language)
    {
        if (_localizedStringsCache.ContainsKey(language))
            return;

        throw new InvalidOperationException(
            $"Localization cache for language {language} is not initialized. " +
            $"Call InitializeAsync first.");
    }

    private async Task AddLocalizationStringIfNeededAsync(
        Language language,
        string resourceIdentifier,
        string localizationKey,
        string defaultValue)
    {
        if (_configuration.AutoAddMissingKeys)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var localizerService = scope.ServiceProvider.GetRequiredService<IJsonStringLocalizerService>();

            await localizerService.AddLocalizedStringAsync(
                language,
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

    //private async Task InitializeDefaultValuesForNonDefaultLanguageAsync(
    //    IJsonStringLocalizerService localizerService,
    //    Language language, 
    //    LocalizationResourceMap targetResourceMap)
    //{
    //    if (language.IsDefaultLanguage() || 
    //        !_configuration.AutoTranslateDefaultValues)
    //    {
    //        return;
    //    }

    //    var initializer = new DefaultResourceInitializer(
    //        localizerService,
    //        language);

    //    await initializer.InitializeAsync(targetResourceMap);
    //}

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
