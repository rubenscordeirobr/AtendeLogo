var builder = DistributedApplication.CreateBuilder(args);

var identityApi = builder.AddProject<Projects.AtendeLogo_IdentityApi>("identityapi");

builder.AddProject<Projects.AtendeLogo_TenantPortal_BlazorServer>("tenant-portal-blazor-server")
    .WithExternalHttpEndpoints()
    .WithReference(identityApi)
    .WaitFor(identityApi);
 
await builder.Build().RunAsync();
