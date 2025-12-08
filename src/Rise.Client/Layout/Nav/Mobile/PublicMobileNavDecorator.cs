using MudBlazor;

namespace Rise.Client.Layout.Nav.Mobile;

public class PublicMobileNavDecorator(INavigationSource inner) : INavigationSource
{
    public HashSet<NavItem> GetItems()
    {
        var items = inner.GetItems();
        
        var lastItem = items.Last();
        items.Remove(lastItem);
        
        items.Add(new NavItem
        {
            Title = "Nieuws", 
            Icon = Icons.Material.Outlined.Newspaper, 
            Href = "/news"
        });
        items.Add(new NavItem
        {
            Title = "Evenementen",
            Icon = Icons.Material.Outlined.Event,
            Href = "/school-events"
        });
        
        items.Add(lastItem);
        
        return items;
    }
}