namespace AtendeLogo.Common.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public sealed class DialingCodeAttribute : Attribute
{
    public string Code { get; }
    public DialingCodeAttribute(string code)
    {
        Code = code;
    }
}
