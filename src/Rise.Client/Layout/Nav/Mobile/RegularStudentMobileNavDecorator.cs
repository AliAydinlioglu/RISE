using MudBlazor;

namespace Rise.Client.Layout.Nav.Mobile;

public class RegularStudentMobileNavDecorator(INavigationSource inner) : INavigationSource
{
    public List<NavItem> GetItems()
    {
        var items = inner.GetItems();

        var lastItem = items.Last();
        items.Remove(lastItem);
        
        items.Add(new NavItem { Title = "Kalender", Icon = Icons.Material.Outlined.CalendarMonth, Href = "/kalender" });
        items.Add(new NavItem { Title = "Activiteiten", Icon = Icons.Material.Outlined.EventNote, Href = "/student-activities" });
        items.Add(lastItem);
        
        return items;
    }
}