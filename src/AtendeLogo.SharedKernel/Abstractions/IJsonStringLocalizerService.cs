using AtendeLogo.Shared.Localization;

namespace AtendeLogo.Shared.Abstractions;

public interface IJsonStringLocalizerService : ICommunicationService
{
    Task<Result<LocalizationResourceMap>> GetLocalizationResourceMapAsync(
        Culture culture, 
        CancellationToken cancellationToken = default);

    Task<Result<LocalizedStrings>> GetLocalizedStringsAsync(
        Culture culture,
        string resourceKey,
        CancellationToken cancellationToken = default);

    Task<Result<OperationResponse>> AddLocalizedStringAsync(
        Culture culture,
        string resourceKey,
        string localizationKey,
        string defaultValue,
        CancellationToken cancellationToken = default);

    Task<Result<OperationResponse>> UpdateDefaultLocalizedStringAsync(
        string resourceKey, 
        string localizationKey, 
        string defaultValue,
        CancellationToken cancellationToken = default);

}
