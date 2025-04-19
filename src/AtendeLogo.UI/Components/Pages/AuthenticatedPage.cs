using Microsoft.AspNetCore.Authorization;

namespace AtendeLogo.UI.Components.Pages;

[Authorize]
public abstract partial class AuthenticatedPage : PageBase
{
}

