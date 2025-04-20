
using AtendeLogo.Application.Extensions;
using AtendeLogo.Shared;

var builder = WebApplication.CreateBuilder(args);
var environment = builder.Environment;

builder.InitializeEnvironmentSettings()
    .ConfigureCors()
    .AddEssentialServices();

var configuration = builder.Configuration;

if (!environment.IsTest())
{
    builder.Services
        .AddInfrastructureServices(configuration, environment)
        .AddIdentityPersistenceServices(configuration)
        .AddActivityPersistenceServices(configuration);
}

builder.Services
    .AddSharedKernelServices()
    .AddApplicationServices()
    .AddRuntimeServices()
    .AddUserCasesSharedServices()
    .AddUserCasesServices()
    .AddPresentationServices();

var app = builder.Build();

#pragma warning disable S125
/* builder.AddServiceDefaults(); */
#pragma warning restore S125

app.UseHttpsRedirection()
   .UseAuthorization();

app.MapControllers();

app.UsePresentationServices();
app.MapPresentationEndPoints();
app.MapFallback();

if (environment.IsDevelopment() && !environment.IsTest())
{
    app.MapOpenApi();

    app.UseSwagger()
       .UseSwaggerUI();

    await app.ApplyMigrationsAsync();
}
await app.RunAsync();
