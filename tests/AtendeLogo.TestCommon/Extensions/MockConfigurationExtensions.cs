using AtendeLogo.TestCommon.Mocks;
using Microsoft.Extensions.Logging;

namespace AtendeLogo.TestCommon.Extensions;

public static class MockConfigurationExtensions
{
    public static IServiceCollection AddMockInfrastructureServices(
        this IServiceCollection services)
    {
        return services.AddSingleton(typeof(IJsonStringLocalizer<>), typeof(JsonStringLocalizerMock<>))
            .AddSingleton<ISecureConfiguration, SecureConfigurationMock>()
            .AddSingleton<ICacheRepository, CacheRepositoryMock>()
            .AddSingleton<IEmailSender, EmailSenderMock>();
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

    public static IServiceCollection AddUserSessionAccessorMock<TRoleProvider>(
        this IServiceCollection services)
        where TRoleProvider : IRoleProvider, new()
    {
        var roleProvider = new TRoleProvider();

        switch (roleProvider.UserRole)
        {
            case UserRole.Anonymous:
                return services.AddSingleton<IHttpContextSessionAccessor, AnonymousUserSessionAccessorMock>();

            case UserRole.Owner:
                return services.AddSingleton<IHttpContextSessionAccessor, TenantOwnerUserSessionAccessorMock>();

            case UserRole.Admin:
                return services.AddSingleton<IHttpContextSessionAccessor, AminUserSessionAccessorMock>();

            default:
                throw new NotImplementedException(
                    $"UserRole {roleProvider.UserRole} not implemented in MockConfigurationExtensions");
        }
    }
}
