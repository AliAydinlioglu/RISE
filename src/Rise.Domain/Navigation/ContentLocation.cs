namespace Rise.Domain.Navigation;

public class ContentLocation : Entity<int>
{
    public string Name { get; private set; } = null!;

    private readonly List<RoleNavigationItemContentLocation> _roleNavigationItems = [];
    public IReadOnlyList<RoleNavigationItemContentLocation> RoleNavigationItems => _roleNavigationItems.AsReadOnly();
    
    private ContentLocation() {}
    
    public ContentLocation(string name)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);
    }
}