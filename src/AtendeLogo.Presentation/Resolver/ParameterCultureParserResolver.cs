using AtendeLogo.Common.Attributes;
using AtendeLogo.Common.Helpers;

namespace AtendeLogo.Presentation.Resolver;

public class ParameterCultureParserResolver : IParameterParserResolver
{
    public object? Parse(string? stringValue)
    {
        return CultureHelper.GetCulture(stringValue);
    }
}
