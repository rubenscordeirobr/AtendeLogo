namespace AtendeLogo.Shared.Localization;

public record JsonLocalizationCacheConfiguration
{
    public bool AutoAddMissingKeys { get; init; }
    public bool AutoUpdateDefaultKeys { get; init; }
    public bool AutoTranslate { get; init; }
    public string? CustomTranslationModelId { get; init; }
}
