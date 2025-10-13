using Rise.Domain.Identity;

namespace Rise.Domain.Navigation;

public class RoleNavigationItem : Entity<RoleNavigationItemId>
{
    public int SequenceNr { get; private set; }
    
    public NavigationItem NavigationItem { get; init; } = null!;
    public Role Role { get; init; } = null!;
    
    private RoleNavigationItem() {}
    
    public RoleNavigationItem(Guid roleId, int navigationItemId, int sequenceNr)
    {
        Id = new RoleNavigationItemId(roleId, navigationItemId);
        SequenceNr = Guard.Against.NegativeOrZero(sequenceNr);
    }
}