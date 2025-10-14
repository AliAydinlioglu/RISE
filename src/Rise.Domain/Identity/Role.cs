using Rise.Domain.Navigation;

namespace Rise.Domain.Identity;

public class Role : Entity<Guid>
{
    public string Name { get; private set; } = null!;
    
    private readonly List<RoleNavigationItemContentLocation>  _roleNavigationItems = [];
    public IReadOnlyList<RoleNavigationItemContentLocation> RoleNavigationItems => _roleNavigationItems.AsReadOnly();
    
    private Role() {}

    public Role(Guid id, string name)
    {
        Id = Guard.Against.Null(id, nameof(id));
        Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
    }
}