using AtendeLogo.Common.Attributes;
using AtendeLogo.Common.Helpers;

namespace AtendeLogo.Presentation.Resolver;

public class ParameterLanguageParserResolver : IParameterParserResolver
{
    public object? Parse(string? stringValue)
    {
        return LanguageHelper.GetLanguage(stringValue);
    }
}
