﻿using AtendeLogo.Common.Helpers;

namespace AtendeLogo.UI.Services;

public class CultureProvider : ICultureProvider
{
    private Culture _culture = CultureHelper.DefaultCulture;

    private readonly IJsonStringLocalizerCache _stringLocalizerCache;

    public CultureProvider(IJsonStringLocalizerCache stringLocalizerCache)
    {
        _stringLocalizerCache = stringLocalizerCache;
    }

    public async Task SetCultureAsync(string cultureCode)
    {
        _culture = CultureHelper.GetCulture(cultureCode);
        await _stringLocalizerCache.LoadCultureAsync(cultureCode);
    }

    public Culture Culture
        => _culture;

    public Country Country
        => CultureHelper.GetCountry(_culture);

    public Language Language
        => CultureHelper.GetLanguage(_culture);

    public Currency Currency
        => CultureHelper.GetCurrency(_culture);
}
