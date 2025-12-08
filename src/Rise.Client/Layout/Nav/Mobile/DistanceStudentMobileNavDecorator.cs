using MudBlazor;

namespace Rise.Client.Layout.Nav.Mobile;

public class DistanceStudentMobileNavDecorator(INavigationSource inner) : INavigationSource
{
    public List<NavItem> GetItems()
    {
        var items = inner.GetItems();

        var lastItem = items.Last();
        items.Remove(lastItem);
        
        items.Add(new NavItem { Title = "Kalender", Icon = Icons.Material.Outlined.CalendarMonth, Href = "/kalender" });
        items.Add(new NavItem { Title = "Campus", Icon = Icons.Material.Outlined.Map, Href = "/campuses" });
        items.Add(lastItem);
        
        return items;
    }
}