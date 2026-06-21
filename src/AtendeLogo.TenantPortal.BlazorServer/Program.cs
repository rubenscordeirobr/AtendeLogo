//using AtendeLogo.ServiceDefaults;
using AtendeLogo.ServiceDefaults;
using AtendeLogo.TenantPortal;
using AtendeLogo.TenantPortal.BlazorServer.Components;
using AtendeLogo.TenantPortal.BlazorServer.Extensions;
using AtendeLogo.TenantPortal.BlazorServer.Services;
using AtendeLogo.TenantPortal.Components.Layout;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);
var environment = builder.Environment;

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpContextAccessor()
    .AddTenantPortalServices()
    .AddScoped<AuthenticationStateProvider, HybridTenantUserAuthenticationStateProvider>()
    .AddScoped<HttpContextSignInService>()
    .AddScoped<HttpContextSignInHandler>()
    .AddScoped<IStorageService, HybridBrowserStorageService>();
    
 
builder.Services.AddAuthentication(TenantUserAuthenticationConfig.AuthenticationScheme)
    .AddCookie(TenantUserAuthenticationConfig.AuthenticationScheme, options =>
    {
        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = context =>
            {
                var culture = LocalizedUriUtils.GetCultureCodeFromUri(context.Request.Path);
                var loginPath = StringFormatUtils.Format(TenantUserAuthenticationConfig.LoginRoute, culture);
                context.Response.Redirect(loginPath);
                return Task.CompletedTask;
            }
        };

        options.LoginPath = TenantUserAuthenticationConfig.LoginRoute;
        options.LogoutPath = TenantUserAuthenticationConfig.LogoutRoute;

        options.ReturnUrlParameter = String.Empty;
        options.AccessDeniedPath = TenantUserAuthenticationConfig.AccessDeniedRoute;
        options.Cookie.Name = TenantUserAuthenticationConfig.AuthenticationScheme;

        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.SlidingExpiration = true;
    });

if (environment.IsAspire())
{
    builder.AddServiceDefaults();
}

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        if (environment.IsDevelopment())
        {
            ctx.Context.Response.Headers.Append("Cache-Control", "no-cache, no-store, must-revalidate");
            ctx.Context.Response.Headers.Append("Pragma", "no-cache");
            ctx.Context.Response.Headers.Append("Expires", "0");
        }
    }
});

app.UseAuthentication()
    .UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(MainLayout).Assembly);

if (environment.IsAspire())
{
   app.MapDefaultEndpoints();
}

app.MapTenantPortalBlazorServerEndpoints();
 
await app.Services.InitializeTenantPortalServiceAsync(environment);
await app.RunAsync();
