using MudBlazor;

namespace Rise.Client.Layout.Nav.Desktop;

public class BaseNavigationSourceDesktop : INavigationSource
{
    public List<NavItem> GetItems()
    {
        return
        [
            new NavItem
            {
                Title = "Home", 
                Icon = Icons.Material.Outlined.Home, 
                Href = "/"
            },
            new NavItem
            {
                Title = "Nieuws", 
                Icon = Icons.Material.Outlined.Newspaper, 
                Href = "/news"
            },
            new NavItem
            {
                Title = "Activiteiten", 
                Icon = Icons.Material.Outlined.EventNote, 
                Href = "/student-activities"
            },
            new NavItem
            {
                Title = "Evenementen", 
                Icon = Icons.Material.Outlined.CalendarToday, 
                Href = "/school-events"
            },
            new NavItem
            {
                Title = "Weekmenu", 
                Icon = Icons.Material.Filled.RestaurantMenu, 
                Href = "/restaurant/weekmenu"
            },
            new NavItem
            {
                Title = "Prijslijst", 
                Icon = Icons.Material.Filled.ReceiptLong, 
                Href = "/restaurant/prijslijst"
            },
            new NavItem
            {
                Title = "Contact", 
                Icon = Icons.Material.Outlined.Person, 
                Href = "/contact"
            },
            new NavItem
            {
                Title = "Campussen", 
                Icon = Icons.Material.Outlined.Map, 
                Href = "/campuses"
            }
        ];
    }
}