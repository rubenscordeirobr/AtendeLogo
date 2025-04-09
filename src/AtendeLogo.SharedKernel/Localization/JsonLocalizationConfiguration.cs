namespace AtendeLogo.Shared.Localization;

public record JsonLocalizationConfiguration : JsonLocalizationCacheConfiguration
{
    public required string ResourcesRootPath { get; init; }
}
