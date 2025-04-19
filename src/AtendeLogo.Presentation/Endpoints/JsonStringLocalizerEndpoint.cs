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
 

    [HttpGet(routeTemplate: "/{culture}")]
    public Task<Result<LocalizationResourceMap>> GetLocalizationResourceMapAsync(
        [ParameterParserResolver<ParameterCultureParserResolver>]
        Culture culture,
        CancellationToken cancellationToken = default)
    {
        return _jsonStringLocalizerService.GetLocalizationResourceMapAsync(culture, cancellationToken);
    }
     
    [HttpGet(routeTemplate: "/{culture}/{*resourceKey}")]
    public Task<Result<LocalizedStrings>> GetLocalizedStringsAsync(
        [ParameterParserResolver<ParameterCultureParserResolver>]
        Culture culture,
        string resourceKey,
        CancellationToken cancellationToken = default)
    {
        return _jsonStringLocalizerService.GetLocalizedStringsAsync(culture, resourceKey, cancellationToken);
    }

    [HttpForm]
    public Task<Result<OperationResponse>> AddLocalizedStringAsync(
        Culture culture,
        string resourceKey,
        string localizationKey,
        string defaultValue,
        CancellationToken cancellationToken = default)
    {
        return _jsonStringLocalizerService.AddLocalizedStringAsync(
            culture,
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
