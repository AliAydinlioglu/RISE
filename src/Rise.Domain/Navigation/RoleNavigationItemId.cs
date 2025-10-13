namespace Rise.Domain.Navigation;

public class RoleNavigationItemId : ValueObject
{
    /// <summary>
    /// Link to the <see cref="IdentityRole"/> role
    /// </summary>
    public Guid RoleId { get; private set; }
    public int NavigationItemId { get; private set; }
    
    private RoleNavigationItemId() {}
    
    public RoleNavigationItemId(Guid roleId, int navigationItemId)
    {
        RoleId = Guard.Against.Null(roleId);
        NavigationItemId = Guard.Against.NegativeOrZero(navigationItemId);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return RoleId;
        yield return NavigationItemId;
    }
}