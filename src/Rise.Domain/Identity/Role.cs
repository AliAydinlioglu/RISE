using Rise.Domain.Navigation;

namespace Rise.Domain.Identity;

public class Role : Entity<Guid>
{
    public string Name { get; private set; } = null!;
    
    private readonly List<RoleNavigationItemContentLocation>  _roleNavigationItems = [];
    public IReadOnlyList<RoleNavigationItemContentLocation> RoleNavigationItems => _roleNavigationItems.AsReadOnly();
    
    private Role() {}

    /// <summary>
    /// Used for fakers
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    public Role(Guid id, string name)
    {
        Id = Guard.Against.NullOrEmpty(id);
        Name = Guard.Against.NullOrWhiteSpace(name);
    }
    
    public Role(string name)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);
    }
}