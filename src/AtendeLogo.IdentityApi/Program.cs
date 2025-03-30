
var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;

builder.InitializeEnvironmentSettings()
    .ConfigureCors()
    .AddEssentialServices();

var configuration = builder.Configuration;

if (!env.IsTest())
{
    builder.Services
        .AddInfrastructureServices(configuration)
        .AddIdentityPersistenceServices(configuration)
        .AddActivityPersistenceServices(configuration);
}
 
builder.Services.AddApplicationServices()
    .AddRuntimeServices()
    .AddUserCasesSharedServices()
    .AddUserCasesServices()
    .AddPresentationServices();

var app = builder.Build();

app.MapDefaultEndpoints();
app.UseHttpsRedirection()
   .UseAuthorization();

app.MapControllers();

app.UsePresentationServices();
app.MapPresentationEndPoints();
app.MapFallback();

if (env.IsDevelopment() && !env.IsTest())
{
    app.MapOpenApi();

    app.UseSwagger()
       .UseSwaggerUI();

    await app.ApplyMigrationsAsync();
}
await app.RunAsync();
