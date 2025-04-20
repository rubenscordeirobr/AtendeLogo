using AtendeLogo.Shared.Abstractions;

namespace AtendeLogo.Shared.Localization;

internal class MissingLocalizationSeeder
{
    private readonly IJsonStringLocalizerService _localizerService;
    private readonly Language _targetLanguage;

    public MissingLocalizationSeeder(
        IJsonStringLocalizerService localizerService,
        Language target)
    {
        _localizerService = localizerService;
        _targetLanguage = LanguageHelper.Normalize(target);
    }

    public async Task<bool> SeedAsync(LocalizationResourceMap targetMap)
    {
        if (_targetLanguage == LanguageHelper.DefaultLanguage)
        {
            return false;
        }

        var sourceMap = await GetDefaultResourceMapAsync();
        return await SeedAsync(sourceMap, targetMap);
    }

    public async Task<bool> SeedAsync(
        LocalizationResourceMap sourceMap,
        LocalizationResourceMap targetMap)
    {
        if (_targetLanguage == LanguageHelper.DefaultLanguage)
        {
            return false;
        }

        var anyResourceChanged = false;

        foreach (var resourceId in sourceMap.Keys)
        {
            var defaultStrings = sourceMap[resourceId];
            var targetStrings = targetMap.GetValueOrDefault(resourceId) ?? new LocalizedStrings();

            var resourceChanged = await SyncMissingLocalizedStringsAsync(
                resourceId,
                defaultStrings,
                targetStrings);

            if (resourceChanged)
            {
                anyResourceChanged = true;
            }
        }
        return anyResourceChanged;
    }

    private async Task<LocalizationResourceMap> GetDefaultResourceMapAsync()
    {
        var result = await _localizerService.GetLocalizationResourceMapAsync(
           LanguageHelper.DefaultLanguage,
           CancellationToken.None);

        if (result.IsFailure)
        {
            throw new InvalidOperationException(
                $"Failed to load default localization resource map. " +
                $"Error: {result.Error.Message}");
        }
        return result.Value;
    }

    private async Task<bool> SyncMissingLocalizedStringsAsync(
        string resourceKey,
        LocalizedStrings defaultStrings,
        LocalizedStrings targetStrings)
    {
        var hasChanges = false;

        foreach (var key in defaultStrings.Keys)
        {
            if (!targetStrings.ContainsKey(key))
            {
                hasChanges = true;

                var defaultValue = defaultStrings[key];
                await _localizerService.AddLocalizedStringAsync(
                    _targetLanguage,
                    resourceKey,
                    key,
                    defaultValue);
            }
        }
        return hasChanges;
    }
}

