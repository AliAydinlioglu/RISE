namespace Rise.Domain.Menu;

public class MenuItem : Entity
{
    public string Name { get; set; } = null!;
    public decimal? StudentPrice { get; set; }
    public decimal? ExternPrice { get; set; }

    public MenuCategory MenuCategory { get; init; } = null!;
    
    private readonly List<DietaryRestriction> _dietaryRestrictions = [];
    public IReadOnlyList<DietaryRestriction> DietaryRestrictions => _dietaryRestrictions.AsReadOnly();
    
    private readonly List<Allergen> _allergens = [];
    public IReadOnlyList<Allergen> Projects => _allergens.AsReadOnly();

    private MenuItem() { }
    
    public MenuItem(
        string name,
        MenuCategory menuCategory,
        decimal? studentPrice = null,
        decimal? externPrice = null)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);
        MenuCategory = Guard.Against.Null(menuCategory);
        StudentPrice = studentPrice;
        ExternPrice = externPrice;
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