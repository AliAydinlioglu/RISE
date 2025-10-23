using Rise.Shared.Navigation;

namespace Rise.Client.Layout
{
    public partial class MainLayout
    {
        protected readonly HashSet<NavigationDto.Get> _navItems =
        [
            new NavigationDto.Get{ Label = "Home", Icon = "fa-solid fa-house", Url = "/" },
            new NavigationDto.Get{Label = "Rooster", Icon = "fa-regular fa-calendar", Url = "/calendar" },
            new NavigationDto.Get{Label = "Studenten", Icon = "fa-solid fa-person-walking-luggage", Url = "/studentactivities" }
        ];
    }
}
