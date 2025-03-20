using AtendeLogo.Application.Contracts.Handlers;
using AtendeLogo.Application.Contracts.Persistence;
using AtendeLogo.Application.Contracts.Services;
using AtendeLogo.Presentation.Common;
using AtendeLogo.Shared.ValueObjects;
using AtendeLogo.UseCases.Common.Services;

namespace AtendeLogo.ArchitectureTests.TestSupport;

public class ServiceReflectionHelper
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
