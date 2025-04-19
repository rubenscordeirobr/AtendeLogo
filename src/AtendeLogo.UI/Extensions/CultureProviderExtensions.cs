using AtendeLogo.UI.Services;

namespace AtendeLogo.UI.Extensions;

public static class CultureProviderExtensions
{
    public static Task SetCultureAsync(
        this ICultureProvider cultureProvider, 
        string cultureCode)
    {
        if (cultureProvider is not CultureProvider concreteProvider)
        {
            throw new InvalidOperationException(
                $"The culture provider must be of type {nameof(CultureProvider)}.");
        }
        return concreteProvider.SetCultureAsync(cultureCode);
    }
}

