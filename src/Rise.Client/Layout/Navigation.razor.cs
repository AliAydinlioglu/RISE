using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Rise.Client.Layout;

public partial class Navigation : ComponentBase
{
 
  
 public HashSet<NavItem> NavItems = new()
  {
    new NavItem { Title = "Home", Icon = Icons.Material.Outlined.Home, Href = "/" },
    new NavItem { Title = "Kalender", Icon = Icons.Material.Outlined.CalendarMonth, Href = "/kalender" },
    new NavItem { Title = "Activiteiten", Icon =Icons.Material.Outlined.EventNote, Href = "/student-activities" },
  };

  [Inject] public NavigationManager MyNavigationManager {get; set;} = null!;
  
  override protected void OnInitialized()
  {
      MyNavigationManager.LocationChanged += (_, _) => StateHasChanged();
  }
  
  private bool IsHomePage()
  {
      var uri = MyNavigationManager.ToBaseRelativePath(MyNavigationManager.Uri);
      return string.IsNullOrEmpty(uri) || uri == "/";
  }
  
 
}
// TODO: Replace with actual navigation items. These are just placeholders.

public class NavItem
{
    public string Title { get; set; } = null!;
    public string Icon { get; set; } = null!;
    public string Href { get; set; } = null!;
}