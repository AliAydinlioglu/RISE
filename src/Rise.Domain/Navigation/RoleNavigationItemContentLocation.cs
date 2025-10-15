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
    
    public RoleNavigationItemContentLocation(Role role, NavigationItem navigationItem, ContentLocation contentLocation, int sequenceNr)
    {
        Role = Guard.Against.Null(role);
        NavigationItem = Guard.Against.Null(navigationItem);
        ContentLocation = Guard.Against.Null(contentLocation);
        
        Id = new RoleNavigationItemContentLocationId(role.Id, navigationItem.Id, contentLocation.Id);
        SequenceNr = Guard.Against.NegativeOrZero(sequenceNr);
    }
}