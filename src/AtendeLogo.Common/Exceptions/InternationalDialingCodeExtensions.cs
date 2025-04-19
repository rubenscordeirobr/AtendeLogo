namespace AtendeLogo.Common.Exceptions;
public static class InternationalDialingCodeExtensions
{
    public static string GetDialingCode(this InternationalDialingCode code)
    {
        return ((int)code).ToString();
    }

    public static string GetDialingCodeString(this InternationalDialingCode code)
    {
        return code.GetDialingCode().ToString();
    }

    public static string ToE164Format(this InternationalDialingCode code)
    {
        return $"+{(int)code}";
    }
}
