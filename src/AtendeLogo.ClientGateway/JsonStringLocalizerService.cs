using System.Text.Json;
using AtendeLogo.Common.Enums;
using AtendeLogo.Common.Extensions;
using AtendeLogo.Common.Utils;
using AtendeLogo.Shared.Abstractions;
using AtendeLogo.Shared.Localization;

namespace AtendeLogo.ClientGateway;

[Route(RouteConstants.StringLocalizerService)]
[JsonOptionsProvider<LocalizationJsonOptionsProvider>]
public class JsonStringLocalizerService : IJsonStringLocalizerService
{
    private readonly IHttpClientMediator<JsonStringLocalizerService> _mediator;

    public JsonStringLocalizerService(IHttpClientMediator<JsonStringLocalizerService> mediator)
    {
        _mediator = mediator;
    }

    public Task<Result<LocalizationResourceMap>> GetLocalizationResourceMapAsync(
        Culture culture,
        CancellationToken cancellationToken = default)
    {
        var route = $"{culture.GetCultureCode()}";
        return _mediator.GetAsync<LocalizationResourceMap>(route, cancellationToken);
    }
     
    public Task<Result<LocalizedStrings>> GetLocalizedStringsAsync(
        Culture culture, 
        string resourceKey, 
        CancellationToken cancellationToken = default)
    {
        var route = $"{culture.GetCultureCode()}/{resourceKey}";
        return _mediator.GetAsync<LocalizedStrings>(route, cancellationToken);
    }

    public Task<Result<OperationResponse>> AddLocalizedStringAsync(
        Culture culture,
        string resourceKey,
        string localizationKey,
        string defaultValue,
        CancellationToken cancellationToken = default)
    {
        return _mediator.FormAsync<OperationResponse>(
            [nameof(culture), nameof(resourceKey), nameof(localizationKey), nameof(defaultValue)],
            [culture, resourceKey, localizationKey, defaultValue],
            cancellationToken);
    }

    public Task<Result<OperationResponse>> UpdateDefaultLocalizedStringAsync(
        string resourceKey,
        string localizationKey,
        string defaultValue,
        CancellationToken cancellationToken = default)
    {
        return _mediator.FormAsync<OperationResponse>(
            [nameof(resourceKey), nameof(localizationKey), nameof(defaultValue)],
            [resourceKey, localizationKey, defaultValue],
            cancellationToken);
    }
}

public class LocalizationJsonOptionsProvider : IJsonOptionsProvider
{
    public JsonSerializerOptions GetJsonOptions()
    {
        return JsonUtils.LocalizationJsonSerializerOptions;
    }
}
