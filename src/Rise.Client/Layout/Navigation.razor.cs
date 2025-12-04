using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using MudBlazor;

namespace Rise.Client.Layout;

public partial class Navigation : ComponentBase
{
    private const string HOME_URL = "/";

    // TODO: Voorlopige oplossing, wordt vervangen door Wim's implementatie van navitems
    public HashSet<NavItem> NavItems = new()
    {
        new NavItem { Title = "Home", Icon = Icons.Material.Outlined.Home, Href = "/" },
        new NavItem { Title = "Kalender", Icon = Icons.Material.Outlined.CalendarMonth, Href = "/kalender" },
        new NavItem { Title = "Activiteiten", Icon = Icons.Material.Outlined.EventNote, Href = "/student-activities" },
        new NavItem { Title = "Evenementen", Icon = Icons.Material.Outlined.CalendarToday, Href = "/school-events" },
        new NavItem { Title = "Contact", Icon = Icons.Material.Outlined.Person, Href = "/contact" },
        new NavItem { Title = "Campussen", Icon = Icons.Material.Outlined.Map, Href = "/campuses" }
    };

    public HashSet<NavItem> MobileNavItems = new()
    {
        new NavItem { Title = "Home", Icon = Icons.Material.Outlined.Home, Href = "/" },
        new NavItem { Title = "Kalender", Icon = Icons.Material.Outlined.CalendarMonth, Href = "/kalender" },
        new NavItem { Title = "Activiteiten", Icon = Icons.Material.Outlined.EventNote, Href = "/student-activities" },
        new NavItem { Title = "Contact", Icon = Icons.Material.Outlined.Person, Href = "/contact" }
    };

    [Inject] public NavigationManager MyNavigationManager { get; set; } = null!;

    override protected void OnInitialized()
    {
        MyNavigationManager.LocationChanged += (_, _) => StateHasChanged();
    }

    private bool IsHomePage()
    {
        var uri = MyNavigationManager.ToBaseRelativePath(MyNavigationManager.Uri);
        return string.IsNullOrEmpty(uri) || uri == "/";
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