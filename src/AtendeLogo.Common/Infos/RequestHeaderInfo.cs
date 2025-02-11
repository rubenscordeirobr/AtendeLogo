namespace AtendeLogo.Common.Infos;
public partial record RequestHeaderInfo(
    string IpAddress,
    string UserAgent,
    string ApplicationName);

public partial record RequestHeaderInfo
{
    public static RequestHeaderInfo Unknown
        => new(IpAddress: "Unknown",
               UserAgent: "Unknown",
               ApplicationName: "Unknown");

    public static RequestHeaderInfo System
        => new(IpAddress: "System",
               UserAgent: "System",
               ApplicationName: "System");
}
