using System.Reflection;
using AtendeLogo.Common.Utils;
using AtendeLogo.Presentation.Common;
using AtendeLogo.Presentation.Common.Exceptions;
using AtendeLogo.Presentation.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

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
            var (routeTemplate, httpVerb) = routeGroup.Key;

            var descriptors = routeGroup.ToArray();

            ValidateDescriptors(endpointType, descriptors);

            string[] httpMethods = [httpVerb.ToString().ToUpper()];

            var route = string.IsNullOrWhiteSpace(routeTemplate)
                ? routePrefix
                : $"{routePrefix}/{routeTemplate.TrimStart('/')}";

            var responseType = descriptors.First().ResponseType;
            var statusCode = (int)descriptors.First().SuccessStatusCode;

            RequestDelegate requestDelegate = async (httpContext) =>
            {
                httpContext.Items.Add(HttpContextItensConstants.EndpointType, endpointType);

                var descriptor = HttpGetDescriptorSelector.Select(httpContext, descriptors);
                var requestHandler = new HttpRequestHandler(
                    httpContext,
                    endpointType,
                    descriptor);

                await requestHandler.HandleAsync();
            };

            var mapBuilder = endpointBuilder.MapMethods(route, httpMethods, requestDelegate)
                .WithMetadata(requestDelegate.Method)
                //.WithGroupName(endpointType.Name)
                .WithMetadata(new EndpointNameMetadata(route))
                .WithMetadata(new HttpMethodMetadata(httpMethods))
                .WithMetadata(new ProducesResponseTypeMetadata(statusCode, responseType));

            if (descriptors.Count() == 1 &&
                descriptors[0].IsBodyParameter)
            {
                mapBuilder.WithMetadata(new AcceptsMetadata(["application/json"], descriptors[0].BodyType));
            }

            var operationParameters = descriptors
                .SelectMany(d => d.OperationParameters)
                .GroupBy(p => p.Name);

            mapBuilder.WithOpenApi(operation =>
            {
                operation.Parameters ??= [];

                foreach (var queryParameter in operationParameters)
                {
                    var schemaType = JsonSchemaUtils.GetJsonSchemaType(queryParameter.First().ParameterType);
                    var operameterLocation = descriptors[0].OperationParameterLocation;

                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Name = queryParameter.Key,
                        In = ParameterLocation.Query,
                        Required = queryParameter.Count() == descriptors.Count(),
                        Schema = new OpenApiSchema
                        {
                            Type = schemaType
                        }
                    });
                }
                return operation;
            });
        }
    }

    private static void ValidateDescriptors(Type endpointType, HttpMethodDescriptor[] descriptors)
    {
        if (descriptors.Length == 1)
        {
            return;
        }

        //if (descriptors.Any(x => x.HttpVerb != Common.Enums.HttpVerb.Get))
        //{
        //    var methodNames = string.Join(", ", descriptors.Select(d => d.Method.Name));
        //    throw new HttpTemplateException(
        //        $"The endpoint {endpointType.Name}." +
        //        $"Multiple endpoint methods sharing the same route template '{descriptors.First().RouteTemplate}' " +
        //        $"are only allowed for GET requests. The following methods use a non-GET verb: {methodNames}.");
        //}

        var distinctQueryTemplateCount = descriptors
            .Select(d => d.OperationTemplate)
            .Distinct()
            .Count();

        if (distinctQueryTemplateCount != descriptors.Length)
        {
            throw new DuplicateEndpointException(
                $"The endpoint '{endpointType.Name}' has multiple methods sharing the same route template '{descriptors.First().RouteTemplate}' " +
                $"and operation template '{descriptors.First().OperationTemplate}'. " +
               $"Each method must have a unique query template.");
        }

        var distinctResponseType = descriptors
            .Select(d => d.ResponseType)
            .Distinct();

        if (distinctResponseType.Count() > 1)
        {
            var methodNames = string.Join(", ", descriptors.Select(d => d.Method.Name));
            var responseTypeNames = string.Join(", ", distinctResponseType.Select(t => t.Name));
            var queries = string.Join(", ", descriptors.Select(d => d.OperationTemplate));

            throw new HttpTemplateException(
                $"The endpoint '{endpointType.Name}' has multiple methods sharing the same route template '{descriptors.First().RouteTemplate}' " +
                $"and operation templates '{queries}', but they must return the same response type. " +
                $"However, the following methods have inconsistent response types: {methodNames} ({responseTypeNames}).");
        }

        //check if has parameter with the same name and different ParameterType

        var distinctParameters = descriptors
            .GroupBy(p => String.Join(",", p.OperationParameters.OrderBy(x => x.Name)))
            .Where(g => g.Count() > 1);

        if (distinctParameters.Any())
        {
            var methodNames = string.Join(", ", descriptors.Select(d => d.Method.Name));
            var parameterNames = string.Join(", ", distinctParameters);

            throw new HttpTemplateException(
                $"The endpoint '{endpointType.Name}' has multiple methods sharing the same route template '{descriptors.First().RouteTemplate}' " +
                $"and operation template '{descriptors.First().OperationTemplate}', but they must have the same parameters. " +
                $"The following parameters are duplicated: {parameterNames} in methods {methodNames}.");
        }
    }
}
