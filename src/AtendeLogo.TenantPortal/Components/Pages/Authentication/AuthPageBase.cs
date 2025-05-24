using AtendeLogo.TenantPortal.Components.Layout;
using AtendeLogo.UI.Components.Pages;
using AtendeLogo.UI.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;

namespace AtendeLogo.TenantPortal.Components.Pages.Authentication;

[Layout(typeof(AuthLayout))]
public abstract class AuthPageBase : AnonymousPage
{
    [Inject]
    protected AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    [Inject]
    protected LocalizedNavigationManager NavigationManager { get; set; }

    [Inject]
    protected IBusyIndicatorService BusyIndicatorService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await CheckAuthenticationStateAsync();
    }

    private async Task CheckAuthenticationStateAsync()
    {
        var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        if (state.IsAuthenticated())
        {
            NavigateToReturnUrl();
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender)
        {
            await CheckAuthenticationStateAsync();
        }
    }

    protected void NavigateToReturnUrl()
    {
        var uri = new Uri(NavigationManager.Uri);
        var query = QueryHelpers.ParseQuery(uri.Query);
        var returnUrl = query.TryGetValue("returnUrl", out var value) ? value.ToString() : "/";
        NavigationManager.NavigateTo(returnUrl);
    }
}

