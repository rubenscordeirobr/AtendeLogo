
namespace AtendeLogo.Common.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class CountryAbbreviationAttribute : Attribute
{
    public string Abbreviation { get; }

    public CountryAbbreviationAttribute(string abbreviation)
    {
        Abbreviation = abbreviation;
    }
}
