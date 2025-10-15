namespace Rise.Domain.Navigation;

public class ContentLocation : Entity<int>
{
    public string Name { get; private set; } = null!;

    private readonly List<RoleNavigationItemContentLocation> _roleNavigationItems = [];
    public IReadOnlyList<RoleNavigationItemContentLocation> RoleNavigationItems => _roleNavigationItems.AsReadOnly();
    
    private ContentLocation() {}
    
    /// <summary>
    /// Used for faker
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    public ContentLocation(int id, string name)
    {
        Id = Guard.Against.NegativeOrZero(id);
        Name = Guard.Against.NullOrWhiteSpace(name);
    }
    
    public ContentLocation(string name)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);
    }
}