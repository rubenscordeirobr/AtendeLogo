using AtendeLogo.Common.Attributes;
using AtendeLogo.Shared.Helpers;

namespace AtendeLogo.Presentation.Resolver;

public class ParameterLanguageParseResolver : IParameterParserResolver
{
    public object? Parse(string? stringValue)
    {
        return LanguageHelper.GetLanguageEnum(stringValue);
    }
}
