using MudBlazor;

namespace Rise.Client.Layout.Nav.Mobile;

public class BaseNavigationSourceMobile : INavigationSource
{
    public HashSet<NavItem> GetItems()
    {
        return
        [
            new NavItem { Title = "Home", Icon = Icons.Material.Outlined.Home, Href = "/" },
            new NavItem { Title = "Contact", Icon = Icons.Material.Outlined.Person, Href = "/contact" }
        ];
    }
}