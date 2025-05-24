using AtendeLogo.TenantPortal.Components.Layout;
using AtendeLogo.UI.Components.Pages;
using Microsoft.AspNetCore.Components;

namespace AtendeLogo.TenantPortal.Components.Pages;

[Layout(typeof(MainLayout))]
public abstract class MainPage : AuthenticatedPage
{
}

