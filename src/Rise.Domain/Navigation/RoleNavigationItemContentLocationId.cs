namespace Rise.Domain.Navigation;

public class RoleNavigationItemContentLocationId : ValueObject
{
    /// <summary>
    /// Link to the <see cref="IdentityRole"/> role
    /// </summary>
    public Guid RoleId { get; private set; }
    public int NavigationItemId { get; private set; }
    public int ContentLocationId { get; private set; }
    
    private RoleNavigationItemContentLocationId() {}
    
    public RoleNavigationItemContentLocationId(Guid roleId, int navigationItemId, int contentLocationId)
    {
        RoleId = Guard.Against.NullOrEmpty(roleId);
        NavigationItemId = Guard.Against.NegativeOrZero(navigationItemId);
        ContentLocationId = Guard.Against.NegativeOrZero(contentLocationId);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return RoleId;
        yield return NavigationItemId;
        yield return ContentLocationId;
    }
}