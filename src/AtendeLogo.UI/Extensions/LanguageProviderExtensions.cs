using AtendeLogo.UI.Services;

namespace AtendeLogo.UI.Extensions;

public static class LanguageProviderExtensions
{
    public static Task SetLanguageAsync(
        this ILanguageProvider languageProvider, 
        string language)
    {
        if (languageProvider is not LanguageProvider concreteProvider)
        {
            throw new InvalidOperationException(
                $"The language provider must be of type {nameof(LanguageProvider)}.");
        }
        return concreteProvider.SetLanguageAsync(language);
    }
}

