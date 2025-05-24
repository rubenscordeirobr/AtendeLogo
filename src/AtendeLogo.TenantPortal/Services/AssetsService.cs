namespace AtendeLogo.TenantPortal.Services;

public class AssetsService : IAssetsService
{
    private readonly IThemeService _themeService;
    private string RootAssetsPath { get; } = @"/_content/AtendeLogo.TenantPortal/assets";

    public AssetsService(IThemeService themeService)
    {
        _themeService = themeService;
    }

    public string ThemeModeSuffix
        => _themeService.IsDarkMode ? "dark" : "light";

    public string LogoPath
        => $"{RootAssetsPath}/images/logo-{ThemeModeSuffix}.webp";
     
  
}
