namespace Rise.Domain.Menu;

public class MenuCategory : Entity
{
    public string Name { get; init; } = null!;
    
    private MenuCategory() { }
    
    public MenuCategory(string name)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);
    }
}