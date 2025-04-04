using AtendeLogo.Application.Abstractions.Handlers;
using AtendeLogo.Application.Abstractions.Persistence;
using AtendeLogo.Application.Abstractions.Services;
using AtendeLogo.Presentation.Common;
using AtendeLogo.Shared.ValueObjects;

namespace AtendeLogo.ArchitectureTests.TestSupport;

public static class ServiceReflectionHelper
{
    private static readonly Type[] ServiceContractInterfaceTypes = [
        typeof(IRepositoryBase<>),
            typeof(IApplicationHandler),
            typeof(IValidator),
            typeof(IValidationService),
            typeof(IApplicationService)
    ];

    public static bool IsImplementService(Type type)
    {
        if (type.ImplementsGenericInterfaceDefinition(typeof(IValidator<>)))
        {
            var validationItemType = type.GetGenericArgumentFromInterfaceDefinition(typeof(IValidator<>));
            if (validationItemType.IsSubclassOf<ValueObjectBase>())
            {
                return false;
            }
        }

        return !type.IsSubclassOf<ApiEndpointBase>()
            && type.IsConcrete()
            && type.IsAssignableTo(ServiceContractInterfaceTypes);
    }
}
