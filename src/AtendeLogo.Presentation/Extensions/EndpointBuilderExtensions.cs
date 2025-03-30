using System.Reflection;
using AtendeLogo.Common.Helpers;
using AtendeLogo.Presentation.Common.Exceptions;
using AtendeLogo.Presentation.Common.Validators;
using AtendeLogo.Presentation.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AtendeLogo.Presentation.Extensions;

public static class EndpointBuilderExtensions
{
    public static void MapPresentationEndPoints(this IEndpointRouteBuilder endpoints)
    {
        var assembly = Assembly.GetExecutingAssembly();
        MapPresentationEndPoints(endpoints, assembly);
    }

    public static void MapPresentationEndPoints(
        this IEndpointRouteBuilder endpointBuilder,
        Assembly assembly)
    {
        Guard.NotNull(assembly);

        var endpointTypes = assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(ApiEndpointBase)))
            .Where(t => !t.IsAbstract && t.GetCustomAttribute<EndPointAttribute>() != null);

        foreach (var endpointType in endpointTypes)
        {
            endpointBuilder.MapEndpointType(endpointType);
        }
    }

    public static void MapEndpointType(
        this IEndpointRouteBuilder endpointBuilder,
        Type endpointType)
    {
        Guard.NotNull(endpointType);

        var endpointAttr = endpointType.GetCustomAttribute<EndPointAttribute>();

        if (endpointAttr is null)
        {
            throw new EndpointAttributeException(
                $"The endpoint type '{endpointType.Name}' must have an EndPointAttribute.");
        }

        var routePrefix = endpointAttr.RoutePrefix;

        var methodDescriptors = endpointType
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(m => m.GetCustomAttribute<HttpMethodAttribute>() != null)
            .Select(m => new HttpMethodDescriptor(m))
            .ToArray();

        var routeGroups = methodDescriptors
            .GroupBy(x => (x.RouteTemplate, x.HttpVerb))
            .ToList();

        foreach (var routeGroup in routeGroups)
        {
            var descriptors = routeGroup.ToArray();

            HttpMethodDescriptorValidator.ValidateDescriptors(endpointType, descriptors);

            var firstDescriptor = descriptors[0];
            var (routeTemplate, httpVerb) = routeGroup.Key;

            string[] httpMethods = [httpVerb.ToString().ToUpperInvariant()];

            var route = RouteHelper.Combine(routePrefix, routeTemplate);

            var responseType = firstDescriptor.ResponseType;
            var statusCode = (int)firstDescriptor.SuccessStatusCode;

            RequestDelegate requestDelegate = async (httpContext) =>
            {
                var descriptor = HttpGetDescriptorSelector.Select(httpContext, descriptors);
                var requestHandler = new HttpRequestExecutor(
                    httpContext,
                    endpointType,
                    descriptor);

                await requestHandler.ProcessRequestAsync();
            };

            var endpointName = MetadataHelpers.FormatEndpointName(routePrefix);
            var acceptsMetadata = MetadataHelpers.GetAcceptsMetadata(firstDescriptor);

            endpointBuilder.MapMethods(route, httpMethods, requestDelegate)
                .WithTags(endpointName)
                .WithMetadata(requestDelegate.Method)
                .WithMetadata(new EndpointNameMetadata($"{route}/{httpVerb.ToString().ToUpperInvariant()}"))
                .WithMetadata(new HttpMethodMetadata(httpMethods))
                .WithMetadata(new ProducesResponseTypeMetadata(statusCode, responseType))
                .WithMetadata(acceptsMetadata)
                .WithOpenApi(operation =>
                 {
                     MetadataHelpers.SetOperationMetadata(operation, descriptors);
                     return operation;
                 });
        }
    }

    public static void MapFallback(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapFallback(async context =>
        {
            var executor = new HttpRequestExecutorFallback(context);
            await executor.ProcessRequestAsync();
       
        });
    }
}
