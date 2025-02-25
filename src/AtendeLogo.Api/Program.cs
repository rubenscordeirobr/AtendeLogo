
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
 
var environment = builder.Environment;

builder.Services
    .AddInfrastructureServices(configuration)
    .AddIdentityPersistenceServices(configuration)
    .AddActivityPersistenceServices(configuration)
    .AddApplicationServices()
    .AddUserCasesSharedServices()
    .AddUserCasesServices()
    .AddPresentationServices();

builder.Services.AddCors(OptionsBuilderConfigurationExtensions =>
{
    OptionsBuilderConfigurationExtensions.AddPolicy("all", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services
    .AddHttpContextAccessor() 
    .AddOpenApi()
    .AddSwaggerGen();
  
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new KebabCaseTransformer()));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger()
       .UseSwaggerUI();

    app.MapOpenApi();

    await app.ApplyMigrationsAsync();
}

app.UseHttpsRedirection()
   .UseAuthorization();

app.MapControllers();

app.UsePresentationServices();
app.MapPresentationEndPoints();
 
app.Run();
