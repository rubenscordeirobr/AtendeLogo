using AtendeLogo.Common.Helpers;

namespace AtendeLogo.UI.Services;

public class CultureProvider : ICultureProvider
{
    private Culture _culture = CultureHelper.DefaultCulture;

    private readonly IJsonStringLocalizerCache _stringLocalizerCache;

    public CultureProvider(IJsonStringLocalizerCache stringLocalizerCache)
    {
        _stringLocalizerCache = stringLocalizerCache;
    }

    public Culture Culture
        => _culture;
   
    public async Task SetCultureAsync(string cultureCode)
    {
        _culture = CultureHelper.GetCulture(cultureCode);
        await _stringLocalizerCache.LoadCultureAsync(cultureCode);
    }

    public Currency Currency
        => throw new NotImplementedException();

    public Country Country => throw new NotImplementedException();
}
