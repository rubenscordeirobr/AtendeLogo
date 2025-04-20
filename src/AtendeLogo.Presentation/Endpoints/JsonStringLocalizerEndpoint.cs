using System.Text.Json;
using AtendeLogo.Common.Attributes;
using AtendeLogo.Common.Enums;
using AtendeLogo.Presentation.Resolver;
using AtendeLogo.Shared.Abstractions;
using AtendeLogo.Shared.Localization;
using Microsoft.AspNetCore.Authorization;

namespace AtendeLogo.Presentation.Endpoints;

[AllowAnonymous]
[EndPoint(RouteConstants.StringLocalizerService)]
public class JsonStringLocalizerEndpoint : ApiEndpointBase, IJsonStringLocalizerService
{
    private readonly IJsonStringLocalizerService _jsonStringLocalizerService;

    public JsonStringLocalizerEndpoint(IJsonStringLocalizerService jsonStringLocalizerService)
    {
        _jsonStringLocalizerService = jsonStringLocalizerService;
    }

    public override JsonSerializerOptions? GetJsonSerializerOptions()
    {
        return JsonUtils.LocalizationJsonSerializerOptions;
    }
 

    [HttpGet(routeTemplate: "/{language}")]
    public Task<Result<LocalizationResourceMap>> GetLocalizationResourceMapAsync(
        [ParameterParserResolver<ParameterLanguageParserResolver>]
        Language language,
        CancellationToken cancellationToken = default)
    {
        return _jsonStringLocalizerService.GetLocalizationResourceMapAsync(language, cancellationToken);
    }
     
    [HttpGet(routeTemplate: "/{language}/{*resourceKey}")]
    public Task<Result<LocalizedStrings>> GetLocalizedStringsAsync(
        [ParameterParserResolver<ParameterLanguageParserResolver>]
        Language language,
        string resourceKey,
        CancellationToken cancellationToken = default)
    {
        return _jsonStringLocalizerService.GetLocalizedStringsAsync(language, resourceKey, cancellationToken);
    }

    [HttpForm]
    public Task<Result<OperationResponse>> AddLocalizedStringAsync(
        Language language,
        string resourceKey,
        string localizationKey,
        string defaultValue,
        CancellationToken cancellationToken = default)
    {
        return _jsonStringLocalizerService.AddLocalizedStringAsync(
            language,
            resourceKey,
            localizationKey,
            defaultValue,
            cancellationToken);
    }

    [HttpForm]
    public Task<Result<OperationResponse>> UpdateDefaultLocalizedStringAsync(
        string resourceKey,
        string localizationKey,
        string defaultValue,
        CancellationToken cancellationToken = default)
    {
        return _jsonStringLocalizerService.UpdateDefaultLocalizedStringAsync(
            resourceKey,
            localizationKey,
            defaultValue,
            cancellationToken);
    }
}
