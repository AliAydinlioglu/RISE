namespace Rise.Domain.Navigation;

public class NavigationItem : Entity<int>
{
    public string Label { get; private set; }
    public string Icon { get; private set; }
    public string Url { get; private set; }
    
    private readonly List<RoleNavigationItemContentLocation> _roleNavigationItems = [];
    public IReadOnlyList<RoleNavigationItemContentLocation> RoleNavigationItems => _roleNavigationItems.AsReadOnly();
    
    private NavigationItem() {}

    public NavigationItem(string label, string icon, string url)
    {
        Label = Guard.Against.NullOrWhiteSpace(label);
        Icon = Guard.Against.NullOrWhiteSpace(icon);
        Url = Guard.Against.NullOrWhiteSpace(url);
    }
}