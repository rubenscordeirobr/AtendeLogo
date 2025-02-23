using AtendeLogo.Application.Contracts.Persistence.Activities;
using AtendeLogo.Persistence.Activity.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace AtendeLogo.Persistence.Activity;

public static class ActivitiyPersistenceServiceConfiguration
{
    public static IServiceCollection AddActivityPersistenceServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IMongoClient>(provider =>
        {
            var connectionString = configuration.GetConnectionString("ActivityMongoDb");
            return new MongoClient(connectionString);
        });

        services.AddScoped(provider =>
        {
            var client = provider.GetRequiredService<IMongoClient>();
            return client.GetDatabase("activityDB");
        });

        services.AddScoped<IActivityRepository, ActivityRepository>();
        return services;
    }
}
