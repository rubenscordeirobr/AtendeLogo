using AtendeLogo.Application.Contracts.Events;
using AtendeLogo.Domain.Primitives;
using Microsoft.Extensions.DependencyInjection;

namespace AtendeLogo.ArchitectureTests.TestSupport;

public class ApplicationServiceProvider : IServiceProvider
{
    private readonly IServiceProvider _serviceProvider;
    public IServiceCollection Services { get; }

    public ApplicationServiceProvider()
    {
        Services = new ApplicationServiceCollection();
        _serviceProvider = Services.BuildServiceProvider();
    }

    public object? GetService(Type serviceType)
    {
        serviceType = NormalizeServiceType(serviceType);
        return _serviceProvider.GetService(serviceType);
    }

    private Type NormalizeServiceType(Type serviceType)
    {
        if (serviceType.IsGenericType && serviceType.ContainsGenericParameters)
        {
            var isEntityStateHandlerType = serviceType
                       .ImplementsGenericInterfaceDefinition(typeof(IEntityStateChangedEventHandler<>)) ||
                       serviceType.ImplementsGenericInterfaceDefinition(typeof(IEntityStateChangedEventPreProcessorHandler<>));

            if (isEntityStateHandlerType)
            {
                return serviceType.MakeGenericType(typeof(EntityBase));
            }

            var genericParameter = serviceType.GetGenericArguments()[0];
            if (genericParameter.IsGenericType && serviceType.ContainsGenericParameters)
            {
                var parameterNormalized = NormalizeServiceType(genericParameter);
                return serviceType.GetGenericTypeDefinition()
                    .MakeGenericType(parameterNormalized);
            }

            throw new InvalidOperationException($" The service {serviceType} has ContainsGenericParameters");

        }
        return serviceType;
    }
}
