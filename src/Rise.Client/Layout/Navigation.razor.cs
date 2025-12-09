using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Rise.Client.Layout.Nav;

namespace Rise.Client.Layout;

public partial class Navigation : ComponentBase
{
    private const string HOME_URL = "/";

    public HashSet<NavItem> NavItems = [];
    public HashSet<NavItem> MobileNavItems = [];

    [Inject] public NavigationManager MyNavigationManager { get; set; } = null!;

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }

    protected override async Task OnInitializedAsync()
    {   
        MyNavigationManager.LocationChanged += (_, _) => StateHasChanged();
        
        var authState = await AuthenticationStateTask;
        var user = authState.User;

        NavItems = NavigationService.GetNavItemsDesktop(user).ToHashSet();
        MobileNavItems = NavigationService.GetNavItemsMobile(user).ToHashSet();
    }

    private bool IsHomePage()
    {
        var uri = MyNavigationManager.ToBaseRelativePath(MyNavigationManager.Uri);
        return string.IsNullOrEmpty(uri) || uri == HOME_URL;
    }

    private NavLinkMatch GetNavLinkMatch(string url)
    {
        return url switch
        {
            HOME_URL => NavLinkMatch.All,
            _ => NavLinkMatch.Prefix
        };
    }
}
// TODO: Voorlopige oplossing, wordt vervangen door Wim's implementatie van navitems.

public class NavItem
{
    public string Title { get; set; } = null!;
    public string Icon { get; set; } = null!;
    public string Href { get; set; } = null!;

}