namespace Rise.Domain.Projects;

/// <summary>
/// A technician is a person who is owner to multiple projects.
/// This is a domain entity and seperated from the <see cref="IdentityUser"/> the link is made via the <see cref="AccountId"/>.
/// So we can swap out the identity provider without changing the domain.
/// </summary>
public class Technician : Entity
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    /// <summary>
    /// Link to the <see cref="IdentityUser"/> account, so a Technician HAS A Account and not IS A <see cref="IdentityUser"/>./>
    /// </summary>
    public string AccountId { get; private set; }

    private readonly List<Project> _projects = [];
    public IReadOnlyList<Project> Projects => _projects.AsReadOnly();

    public Technician(string firstName, string lastName, string accountId)
    {
        FirstName = Guard.Against.NullOrEmpty(firstName);
        LastName = Guard.Against.NullOrEmpty(lastName);
        AccountId = Guard.Against.NullOrEmpty(accountId);
    }
    
    
}