namespace AtendeLogo.Common.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class CountryNumericCodeAttribute : Attribute
{
    public int NumericCode { get; }

    public CountryNumericCodeAttribute(int numericCode)
    {
        NumericCode = numericCode;
    }
}
