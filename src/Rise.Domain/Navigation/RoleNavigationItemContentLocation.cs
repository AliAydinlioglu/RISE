using Rise.Domain.Identity;

namespace Rise.Domain.Navigation;

public class RoleNavigationItemContentLocation : Entity<RoleNavigationItemContentLocationId>
{
    public int SequenceNr { get; private set; }
    
    public NavigationItem NavigationItem { get; init; } = null!;
    public Role Role { get; init; } = null!;
    public ContentLocation ContentLocation { get; init; } = null!;
    
    private RoleNavigationItemContentLocation() {}
    
    public RoleNavigationItemContentLocation(Guid roleId, int navigationItemId, int contentLocationId, int sequenceNr)
    {
        Id = new RoleNavigationItemContentLocationId(roleId, navigationItemId, contentLocationId);
        SequenceNr = Guard.Against.NegativeOrZero(sequenceNr);
    }
}