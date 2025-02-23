using AtendeLogo.Application.Contracts.Security;
using AtendeLogo.Persistence.Identity;
using AtendeLogo.Shared.Contracts;
using Microsoft.Extensions.Logging;

namespace AtendeLogo.Application.UnitTests.Mocks;
public static class MockConfigurationExtensions
{
    public static IServiceCollection AddMockInfrastructureServices(
        this IServiceCollection services,
        bool isAnonymousUserSession)
    {
        services.AddSingleton(typeof(IJsonStringLocalizer<>), typeof(JsonStringLocalizer<>))
            .AddSingleton<ISecureConfiguration, SecureConfigurationMock>()
            .AddSingleton<ISessionCacheService, SessionCacheServiceMock>()
            .AddSingleton<ICommandTrackingService, CommandTrackingServiceMock>()
            .AddSingleton<IEmailSender, EmailSenderMock>();

        if (isAnonymousUserSession)
        {
            services.AddSingleton<IRequestUserSessionService, AnonymousUserSessionServiceMock>();
        }
        else
        {
            services.AddSingleton<IRequestUserSessionService, SystemTenantUserSessionServiceMock>();
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
