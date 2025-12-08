using MudBlazor;

namespace Rise.Client.Layout.Nav.Desktop;

public class AuthorizedDesktopNavDecorator(INavigationSource inner) : INavigationSource
{
    public HashSet<NavItem> GetItems()
    {
        var items = inner.GetItems();
        
        items.Add(new NavItem { Title = "Kalender", Icon = Icons.Material.Outlined.CalendarMonth, Href = "/kalender" });

        return items;
    }
}