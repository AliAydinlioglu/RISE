namespace Rise.Domain.Navigations;

public class RoleNavigationItem : Entity
{
    /// <summary>
    /// Link to the <see cref="IdentityRole"/> role
    /// </summary>
    public Guid RoleId { get; private set; }
    public int NavigationItemId { get; private set; }
    public int PlatformTypeId { get; private set; }
    public int SequenceNr { get; private set; }
    
    public NavigationItem NavigationItem { get; init; } = null!;
    public PlatformType PlatformType { get; init; } = null!;
    
    private RoleNavigationItem() {}

    public RoleNavigationItem(Guid roleId, int navigationItemId, int platformTypeId, int sequenceNr)
    {
        RoleId = Guard.Against.Null(roleId);
        NavigationItemId = Guard.Against.NegativeOrZero(navigationItemId);
        PlatformTypeId = Guard.Against.NegativeOrZero(platformTypeId);
        SequenceNr = Guard.Against.NegativeOrZero(sequenceNr);
    }
}