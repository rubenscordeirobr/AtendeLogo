namespace AtendeLogo.TenantPortal.Services;

public class TenantPortalApplicationInfo : IApplicationInfo
{
    public string ApplicationName 
        => "AtendeLogo.TenantPortal";

    public Version ApplicationVersion
        => new Version("0.0.0.1");

    public string Environment 
        => "Development";

    public string Title 
        => "Portal AtendeLogo";
}
