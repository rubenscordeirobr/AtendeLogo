using System.Reflection;
using AtendeLogo.Presentation.Common;
using AtendeLogo.Presentation.Common.Exceptions;
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
        var endpointAttr = endpointType.GetCustomAttribute<EndPointAttribute>();
        if(endpointAttr is null)
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
            var (routeTemplate, httpVerb) = routeGroup.Key;

            var descriptors = routeGroup.ToArray();

            ValidateDescriptors(descriptors);

            string[] httpMethods = [httpVerb.ToString().ToUpper()];

            var route = string.IsNullOrWhiteSpace(routeTemplate)
                ? routePrefix
                : $"{routePrefix}/{routeTemplate.TrimStart('/')}";

            RequestDelegate requestDelegate = async (httpContext) =>
            {
                var descriptor = HttpGetDescriptorSelector.Select(httpContext, descriptors);

                var requestHandler = new HttpRequestHandler(
                    httpContext,
                    endpointType,
                    descriptor);

                await requestHandler.HandleAsync();
            };
            endpointBuilder.MapMethods(route, httpMethods, requestDelegate);
        }
    }

    private static void ValidateDescriptors(HttpMethodDescriptor[] descriptors)
    {
        if (descriptors.Length == 1)
        {
            return;
        }

        var distinctQueryTemplateCount = descriptors
            .Select(d => d.QueryTemplate)
            .Distinct()
            .Count();

        if (distinctQueryTemplateCount != descriptors.Length)
        {
            throw new DuplicateEndpointException(
                $"Multiple methods with the same route template '{descriptors.First().RouteTemplate}' " +
                $"and query template '{descriptors.First().QueryTemplate}' are not allowed.");
        }
    }
}
