﻿using AtendeLogo.Shared.Abstractions;
using AtendeLogo.Shared.Helpers;

namespace AtendeLogo.Shared.Localization;

public class DefaultResourceInitializer
{
    private readonly IJsonStringLocalizerService _localizerService;
    private readonly Language _targetLanguage;

    public DefaultResourceInitializer(
        IJsonStringLocalizerService localizerService,
        Language targetLanguage)
    {
        _localizerService = localizerService;
        _targetLanguage = LanguageHelper.Normalize(targetLanguage);
    }

    public async Task<bool> InitializeAsync(LocalizationResourceMap targetMap)
    {
        if (_targetLanguage == LanguageHelper.DefaultLanguage)
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

