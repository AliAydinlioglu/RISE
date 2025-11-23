namespace Rise.Domain.Menu;

public class MenuItem : Entity
{
    public string Name { get; set; } = null!;
    public Price Price { get; set; } = null!;

    public MenuCategory MenuCategory { get; init; } = null!;
    
    private readonly List<DietaryRestriction> _dietaryRestrictions = [];
    public IReadOnlyList<DietaryRestriction> DietaryRestrictions => _dietaryRestrictions.AsReadOnly();
    
    private readonly List<Allergen> _allergens = [];
    public IReadOnlyList<Allergen> Allergens => _allergens.AsReadOnly();

    private MenuItem() { }
    
    public MenuItem(
        string name,
        MenuCategory menuCategory,
        Price price)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);
        MenuCategory = Guard.Against.Null(menuCategory);
        Price = Guard.Against.Null(price);
    }

    public void AddDietaryRestrictions(IEnumerable<DietaryRestriction> dietaryRestrictions)
    {
        foreach (var restriction in dietaryRestrictions)
        {
            _dietaryRestrictions.Add(Guard.Against.Null(restriction));
        }
    }

    public void AddAllergens(IEnumerable<Allergen> allergens)
    {
        foreach (var allergen in allergens)
        {
            _allergens.Add(Guard.Against.Null(allergen));
        }
    }
}