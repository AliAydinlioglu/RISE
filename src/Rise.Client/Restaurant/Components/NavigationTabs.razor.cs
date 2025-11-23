using Microsoft.AspNetCore.Components;

namespace Rise.Client.Restaurant.Components;

public partial class NavigationTabs : ComponentBase
{
    [Parameter] public NavigationTab ActiveTab { get; set; }
    
    private string GetTabUrl(NavigationTab tab) =>
        tab switch
        {
            NavigationTab.Weekmenu => "/restaurant/",
            NavigationTab.Prijslijst => "/restaurant/prijslijst",
            _ => "/restaurant/"
        };
}

public enum NavigationTab
{
    Weekmenu,
    Prijslijst
}