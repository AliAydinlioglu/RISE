namespace Rise.Domain.Navigations;

public class PlatformType : Entity
{
    public string Name { get; private set; }

    private readonly List<RoleNavigationItem> _roleNavigationItems = [];
    public IReadOnlyList<RoleNavigationItem> RoleNavigationItems => _roleNavigationItems.AsReadOnly();
    
    private PlatformType() { }

    public PlatformType(string name)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);
    }
}