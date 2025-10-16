namespace Rise.Domain.Navigation;

public class NavigationItem : Entity<int>
{
    public string Label { get; private set; }
    public string Icon { get; private set; }
    public string Url { get; private set; }
    public string Description{ get; set; } = string.Empty;

    private readonly List<RoleNavigationItemContentLocation> _roleNavigationItems = [];
    public IReadOnlyList<RoleNavigationItemContentLocation> RoleNavigationItems => _roleNavigationItems.AsReadOnly();
    
    private NavigationItem() {}

    /// <summary>
    /// Used for Fakers
    /// </summary>
    /// <param name="id"></param>
    /// <param name="label"></param>
    /// <param name="icon"></param>
    /// <param name="url"></param>
    public NavigationItem(int id, string label, string icon, string url)
    {
        Id = Guard.Against.NegativeOrZero(id);
        Label = Guard.Against.NullOrWhiteSpace(label);
        Icon = Guard.Against.NullOrWhiteSpace(icon);
        Url = Guard.Against.NullOrWhiteSpace(url);
    }
    
    public NavigationItem(string label, string icon, string url)
    {
        Label = Guard.Against.NullOrWhiteSpace(label);
        Icon = Guard.Against.NullOrWhiteSpace(icon);
        Url = Guard.Against.NullOrWhiteSpace(url);
    }
}