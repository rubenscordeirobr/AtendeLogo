using AtendeLogo.Shared.Abstractions;

namespace AtendeLogo.Shared.Localization;

public class DefaultResourceInitializer
{
    private readonly IJsonStringLocalizerService _localizerService;
    private readonly Culture _targetCulture;

    public DefaultResourceInitializer(
        IJsonStringLocalizerService localizerService,
        Culture targetCulture)
    {
        _localizerService = localizerService;
        _targetCulture = CultureHelper.Normalize(targetCulture);
    }

    public async Task<bool> InitializeAsync(LocalizationResourceMap targetMap)
    {
        if (_targetCulture == CultureHelper.DefaultCulture)
        {
            return false;
        }

        var defaultMap = await GetDefaultResourceMapAsync();
        var anyResourceChanged = false;

        foreach (var resourceId in defaultMap.Keys)
        {
            var defaultStrings = defaultMap[resourceId];
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
           CultureHelper.DefaultCulture,
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
                    _targetCulture,
                    resourceKey,
                    key,
                    defaultValue);
            }
        }
        return hasChanges;
    }
}

