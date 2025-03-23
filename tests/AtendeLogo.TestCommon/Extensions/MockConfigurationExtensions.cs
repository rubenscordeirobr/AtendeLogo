using AtendeLogo.TestCommon.Mocks;
using Microsoft.Extensions.Logging;

namespace AtendeLogo.TestCommon.Extensions;

public static class MockConfigurationExtensions
{
    public static IServiceCollection AddMockInfrastructureServices<TRoleProvider>(
        this IServiceCollection services)
        where TRoleProvider : IRoleProvider, new()
    {
        services.AddSingleton(typeof(IJsonStringLocalizer<>), typeof(JsonStringLocalizerMock<>))
            .AddSingleton<ISecureConfiguration, SecureConfigurationMock>()
            .AddSingleton<ICacheRepository, CacheRepositoryMock>()
            .AddSingleton<IEmailSender, EmailSenderMock>();

        var roleProvider = new TRoleProvider();
       
        switch (roleProvider.UserRole)
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
                    $"UserRole {roleProvider.UserRole} not implemented in MockConfigurationExtensions");
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
        return services.AddTransient(typeof(ILogger<>), typeof(TestOutputLogger<>));
    }
}
