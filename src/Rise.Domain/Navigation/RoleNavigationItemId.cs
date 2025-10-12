namespace Rise.Domain.Navigation;

public class RoleNavigationItemId : ValueObject
{
    /// <summary>
    /// Link to the <see cref="IdentityRole"/> role
    /// </summary>
    public string RoleId { get; private set; }
    public int NavigationItemId { get; private set; }
    
    private RoleNavigationItemId() {}
    
    public RoleNavigationItemId(string roleId, int navigationItemId)
    {
        RoleId = Guard.Against.RoleIdNullOrWhitespaceOrInvalidGuid(roleId);
        NavigationItemId = Guard.Against.NegativeOrZero(navigationItemId);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return RoleId;
        yield return NavigationItemId;
    }
}

public static class RoleNavigationItemGuards
{
    public static string RoleIdNullOrWhitespaceOrInvalidGuid(this IGuardClause guardClause, string roleId)
    {
        if (string.IsNullOrWhiteSpace(roleId))
            throw new ArgumentException("RoleId cannot be null or empty", nameof(roleId));

        if (!Guid.TryParse(roleId, out var roleGuid))
            throw new ArgumentException("RoleId is in an invalid form", nameof(roleId));

        return roleId;
    }
}