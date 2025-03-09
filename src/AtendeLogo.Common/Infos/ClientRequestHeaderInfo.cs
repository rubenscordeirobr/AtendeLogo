namespace AtendeLogo.Common.Infos;
public partial record ClientRequestHeaderInfo(
    string IpAddress,
    string UserAgent,
    string ApplicationName);

public partial record ClientRequestHeaderInfo
{
    public static ClientRequestHeaderInfo Unknown
        => new(IpAddress: "Unknown",
               UserAgent: "Unknown",
               ApplicationName: "Unknown");

    public static ClientRequestHeaderInfo System
        => new(IpAddress: "System",
               UserAgent: "System",
               ApplicationName: "System");
}
