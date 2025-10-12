namespace Rise.Domain.Navigation;

public class RoleNavigationItem : Entity<RoleNavigationItemId>
{
    public int SequenceNr { get; private set; }
    
    public NavigationItem NavigationItem { get; init; } = null!;
    
    private RoleNavigationItem() {}

    public RoleNavigationItem(RoleNavigationItemId id, int sequenceNr)
    {
        Id = id;
        SequenceNr = Guard.Against.NegativeOrZero(sequenceNr);
    }
}