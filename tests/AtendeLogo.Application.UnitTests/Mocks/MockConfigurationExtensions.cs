using AtendeLogo.Application.Contracts.Security;
using AtendeLogo.Persistence.Identity;
using AtendeLogo.Shared.Contracts;
using Microsoft.Extensions.Logging;

namespace AtendeLogo.Application.UnitTests.Mocks;

public static class MockConfigurationExtensions
{
    public static IServiceCollection AddMockInfrastructureServices(
        this IServiceCollection services,
        UserRole userRole)
    {
        services.AddSingleton(typeof(IJsonStringLocalizer<>), typeof(JsonStringLocalizerMock<>))
            .AddSingleton<ISecureConfiguration, SecureConfigurationMock>()
            .AddSingleton<ICacheRepository, CacheRepositoryMock>()
            .AddSingleton<IEmailSender, EmailSenderMock>();

        switch (userRole)
        {
            case UserRole.Anonymous:
                services.AddSingleton<IUserSessionAccessor, AnonymousUserSessionAccessorMock>();
                break;
            case UserRole.Owner:
                services.AddSingleton<IUserSessionAccessor, TenantOwnerUserSessionAccessorMock>();
                break;
            case UserRole.SystemAdmin:
                services.AddSingleton<IUserSessionAccessor, SystemAdminUserSessionAccessorMock>();
                break;
            default:
                throw new NotImplementedException(
                    $"UserRole {userRole} not implemented in MockConfigurationExtensions");
        }
        return services;
    }

    public static IServiceCollection AddPersistenceServicesMock(
    this IServiceCollection services)
    {
        return services
            .AddSingleton<IActivityRepository, ActivityRepositoryMock>()
            .AddIdentyRepositoryServices();
    }

    public static IServiceCollection AddLoggerServiceMock(
        this IServiceCollection services)
    {
        return services.AddTransient(typeof(ILogger<>), typeof(LoggerServiceMock<>));
    }
}
